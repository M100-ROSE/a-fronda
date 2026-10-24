using System.Text;
using System.Text.Json;
using Fronda.Auth;
using Fronda.Auth.Armazenamento;
using Fronda.Auth.Hash;
using Xunit;

namespace Fronda.Auth.Tests;

/// <summary>Propriedades criptográficas do hasher local.</summary>
public class Pbkdf2HasherTests
{
    private readonly Pbkdf2Hasher _hasher = new();

    [Fact]
    public void MesmaSenha_GeraSaltsDiferentes()
    {
        var (hashA, saltA) = _hasher.Gerar("aSenha#123");
        var (hashB, saltB) = _hasher.Gerar("aSenha#123");

        Assert.Equal(16, Convert.FromBase64String(saltA).Length); // salt de 16 bytes
        Assert.Equal(32, Convert.FromBase64String(hashA).Length); // chave de 32 bytes
        Assert.NotEqual(saltA, saltB);   // salt aleatório por conta
        Assert.NotEqual(hashA, hashB);   // hash muda junto com o salt
    }

    [Fact]
    public void HashNaoContemASenha()
    {
        const string senha = "aSenha#123";
        var (hash, salt) = _hasher.Gerar(senha);

        var hashBytes = Encoding.UTF8.GetBytes(hash);
        var senhaBytes = Encoding.UTF8.GetBytes(senha);
        Assert.False(hashBytes.AsSpan().IndexOf(senhaBytes) >= 0); // sem substring da senha
        Assert.DoesNotContain(senha, hash);
        Assert.DoesNotContain(senha, salt);
    }

    [Fact]
    public void Conferir_SenhaCertaSimErradaNao()
    {
        var (hash, salt) = _hasher.Gerar("aSenha#123");

        Assert.True(_hasher.Conferir("aSenha#123", hash, salt));
        Assert.False(_hasher.Conferir("aSenha#124", hash, salt));
    }

    [Fact]
    public void Conferir_DadosCorrompidos_NuncaBate()
    {
        Assert.False(_hasher.Conferir("aSenha#123", "nao-base64!!", Convert.ToBase64String(new byte[16])));
        Assert.False(_hasher.Conferir("aSenha#123", Convert.ToBase64String(new byte[32]), "erro!!"));
    }

    [Fact]
    public void ContaPersistida_NaoExpoeSenhaNoArquivo()
    {
        // caminho próprio por teste — sem depender do %APPDATA% real
        var caminho = Path.Combine(Path.GetTempPath(),
            $"fronda-tests-{Guid.NewGuid():N}", "usuarios.json");
        var repo = new JsonContasRepository(caminho);
        var (hash, salt) = _hasher.Gerar("aSenha#123");
        repo.Adicionar(new Conta
        {
            Usuario = "raio",
            Hash = hash,
            Salt = salt,
            Lado = Lado.Reino,
            DataCriacao = DateTime.UtcNow,
        });

        var texto = File.ReadAllText(caminho);
        Assert.DoesNotContain("aSenha#123", texto); // nunca em texto puro

        using var doc = JsonDocument.Parse(texto);
        foreach (var conta in doc.RootElement.EnumerateArray())
        {
            Assert.True(conta.TryGetProperty("hash", out _));
            Assert.False(conta.TryGetProperty("senha", out _)); // campo proibido
        }
    }
}
