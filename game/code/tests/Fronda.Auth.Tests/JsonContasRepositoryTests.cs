using System.Text.Json;
using Fronda.Auth;
using Fronda.Auth.Armazenamento;
using Xunit;

namespace Fronda.Auth.Tests;

/// <summary>Comportamento do arquivo de contas: tolerância, atomicidade, formato e ids.</summary>
public class JsonContasRepositoryTests : TesteComArquivoTemporario
{
    [Fact]
    public void ArquivoAusente_ComecaVazio()
    {
        var repo = NovoRepositorio();

        Assert.False(File.Exists(Caminho)); // leitura não cria nada
        Assert.Null(repo.BuscarPorUsuario("raio"));
    }

    [Fact]
    public void ArquivoCorrompido_ComecaVazioEFalhaDeGravarRecria()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(Caminho)!);
        File.WriteAllText(Caminho, "{ isto não é json ...");

        var repo = NovoRepositorio();

        Assert.Null(repo.BuscarPorUsuario("raio")); // tolerante à leitura corrompida

        // e a próxima gravação substitui o lixo por um arquivo válido
        repo.Adicionar(new Conta
        {
            Usuario = "raio", Hash = "xyz", Salt = "abc", Lado = Lado.Reino,
            DataCriacao = DateTime.UtcNow,
        });
        Assert.NotEqual("{ isto não é json ...", File.ReadAllText(Caminho));
    }

    [Fact]
    public void Adicionar_PersisteEmArquivoESobreviveAOutraInstancia()
    {
        var repo = NovoRepositorio();
        var conta = repo.Adicionar(new Conta
        {
            Usuario = "raio", Hash = "xyz", Salt = "abc", Lado = Lado.Oposicao,
            DataCriacao = DateTime.UtcNow,
        });

        var outra = new JsonContasRepository(Caminho);

        Assert.Equal(1, conta.Id); // id sequencial começa em 1
        var lida = outra.BuscarPorUsuario("raio");
        Assert.NotNull(lida);
        Assert.Equal(conta.Id, lida!.Id);
        Assert.Equal("xyz", lida.Hash);
        Assert.Equal("abc", lida.Salt);
        Assert.Equal(Lado.Oposicao, lida.Lado);
    }

    [Fact]
    public void IdsSequenciais_MesmoDepoisDeReabrir()
    {
        var repo = NovoRepositorio();
        Conta Nova(int i) => new()
        {
            Usuario = $"raio{i}", Hash = "xyz", Salt = "abc", Lado = Lado.Reino,
            DataCriacao = DateTime.UtcNow,
        };

        Assert.Equal(1, repo.Adicionar(Nova(1)).Id);
        Assert.Equal(2, repo.Adicionar(Nova(2)).Id);

        var reaberto = new JsonContasRepository(Caminho);
        Assert.Equal(3, reaberto.Adicionar(Nova(3)).Id);
    }

    [Fact]
    public void GravacaoAtomica_TmpAbandonadoAindaGravaArquivoValido()
    {
        // simulando um crash de gravação anterior: um .tmp abandonado no diretório
        Directory.CreateDirectory(Path.GetDirectoryName(Caminho)!);
        File.WriteAllText(Caminho + ".tmp", "lixo parcial");
        // arquivo final inexistente → leitura começa vazia
        var repo = NovoRepositorio();
        repo.Adicionar(new Conta
        {
            Usuario = "camada", Hash = "zz", Salt = "yy", Lado = Lado.Reino,
            DataCriacao = DateTime.UtcNow,
        });

        var final = File.ReadAllText(Caminho);
        Assert.Contains("camada", final);
        using var doc = JsonDocument.Parse(final); // array JSON válido
        Assert.Equal(JsonValueKind.Array, doc.RootElement.ValueKind);
    }

    [Fact]
    public void GravacaoNaoDeixaArquivoTemporarioParaTras()
    {
        var repo = NovoRepositorio();
        repo.Adicionar(new Conta
        {
            Usuario = "raio", Hash = "xyz", Salt = "abc", Lado = Lado.Reino,
            DataCriacao = DateTime.UtcNow,
        });

        var pendente = Directory.GetFiles(Path.GetDirectoryName(Caminho)!, "*.tmp");
        Assert.Empty(pendente); // gravação atômica não deixa lixo
    }

    [Fact]
    public void FormatoPersistido_CamelCaseLadoComoTexto()
    {
        var repo = NovoRepositorio();
        repo.Adicionar(new Conta
        {
            Usuario = "raio", Hash = "xyz", Salt = "abc", Lado = Lado.Reino,
            DataCriacao = DateTime.UtcNow,
        });

        var texto = File.ReadAllText(Caminho);
        Assert.Contains("\"usuario\": \"raio\"", texto);   // camelCase
        Assert.Contains("\"lado\": \"reino\"", texto);     // enum como "reino"|"oposicao"
        Assert.Contains("\"dataCriacao\"", texto);
        Assert.Contains("\"hash\"", texto);
        Assert.Contains("\"salt\"", texto);
        Assert.Contains("\"id\"", texto);
    }
}
