using Fronda.Auth;
using Fronda.Auth.Armazenamento;
using Xunit;

namespace Fronda.Auth.Tests;

/// <summary>Casos de Registrar/Entrar do serviço (validações e persistência).</summary>
public class AuthServiceTests : TesteComArquivoTemporario
{
    // ------------------------------------------------------------- registrar

    [Fact]
    public async Task RegistrarFeliz_PersisteContaERetornaJogador()
    {
        var (servico, repo) = NovoServico();

        var res = await servico.Registrar("raio", "aSenha#123", "aSenha#123", Lado.Reino);

        Assert.True(res.Sucesso);
        Assert.NotNull(res.Jogador);
        Assert.True(res.Jogador!.Id > 0);
        Assert.Equal("raio", res.Jogador.Usuario);
        Assert.Equal(Lado.Reino, res.Jogador.Lado);

        // persistiu de verdade: outra instância do repositório (mesmo arquivo) encontra
        var conta = new JsonContasRepository(Caminho).BuscarPorUsuario("raio");
        Assert.NotNull(conta);
        Assert.Equal(res.Jogador.Id, conta!.Id);
        Assert.Equal(Lado.Reino, conta.Lado);
        Assert.NotEqual("aSenha#123", conta.Hash); // nunca em texto puro
    }

    [Fact]
    public async Task RegistrarNormalizaParaMinusculas()
    {
        var (servico, repo) = NovoServico();

        var res = await servico.Registrar(" RaIO ", "aSenha#123", "aSenha#123", Lado.Oposicao);

        Assert.True(res.Sucesso);
        Assert.Equal("raio", res.Jogador!.Usuario);
        Assert.NotNull(repo.BuscarPorUsuario("raio")); // persistiu já normalizado
    }

    [Fact]
    public async Task RegistrarUsuarioCurto()
    {
        var (servico, _) = NovoServico();
        var res = await servico.Registrar("ab", "aSenha#123", "aSenha#123", Lado.Reino);
        Assert.False(res.Sucesso);
        Assert.Equal("usuario", res.ErroCampo);
        Assert.Equal("usuario_curto", res.ErroCodigo);
    }

    [Fact]
    public async Task RegistrarUsuarioVazio()
    {
        var (servico, _) = NovoServico();
        var res = await servico.Registrar("", "aSenha#123", "aSenha#123", Lado.Reino);
        Assert.Equal("usuario_curto", res.ErroCodigo);
    }

    [Fact]
    public async Task RegistrarUsuarioLongo()
    {
        var (servico, _) = NovoServico();
        var res = await servico.Registrar(new string('a', 21), "aSenha#123", "aSenha#123", Lado.Reino);
        Assert.Equal("usuario_longo", res.ErroCodigo);
        Assert.Equal("usuario", res.ErroCampo);
    }

    [Fact]
    public async Task RegistrarUsuarioComCaracteresInvalidos()
    {
        var (servico, _) = NovoServico();
        var res = await servico.Registrar("ra io", "aSenha#123", "aSenha#123", Lado.Reino);
        Assert.Equal("usuario_invalido", res.ErroCodigo);
    }

    [Fact]
    public async Task RegistrarSenhaCurta()
    {
        var (servico, _) = NovoServico();
        var res = await servico.Registrar("raio", "curta12", "curta12", Lado.Reino);
        Assert.Equal("senha", res.ErroCampo);
        Assert.Equal("senha_curta", res.ErroCodigo);
    }

    [Fact]
    public async Task RegistrarSenhasDiferentes()
    {
        var (servico, _) = NovoServico();
        var res = await servico.Registrar("raio", "aSenha#123", "outraSenha#9", Lado.Reino);
        Assert.Equal("confirmaSenha", res.ErroCampo);
        Assert.Equal("senha_diferente", res.ErroCodigo);
    }

    [Fact]
    public async Task RegistrarLadoInvalido()
    {
        var (servico, _) = NovoServico();
        var res = await servico.Registrar("raio", "aSenha#123", "aSenha#123", (Lado)7);
        Assert.Equal("lado", res.ErroCampo);
        Assert.Equal("lado_obrigatorio", res.ErroCodigo);
    }

    [Fact]
    public async Task RegistrarUsuarioDuplicado_IndependenteDeCaixa()
    {
        var (servico, _) = NovoServico();
        Assert.True((await servico.Registrar("raio", "aSenha#123", "aSenha#123", Lado.Reino)).Sucesso);

        var res = await servico.Registrar("RAIO", "outraSenha#1", "outraSenha#1", Lado.Oposicao);

        Assert.False(res.Sucesso);
        Assert.Equal("usuario", res.ErroCampo);
        Assert.Equal("usuario_em_uso", res.ErroCodigo);
    }

    // ---------------------------------------------------------------- entrar

    [Fact]
    public async Task EntrarFeliz_RetornaJogador()
    {
        var (servico, _) = NovoServico();
        var resRegistro = await servico.Registrar("raio", "aSenha#123", "aSenha#123", Lado.Oposicao);

        var res = await servico.Entrar("raio", "aSenha#123");

        Assert.True(res.Sucesso);
        Assert.Equal(resRegistro.Jogador!.Id, res.Jogador!.Id);
        Assert.Equal("raio", res.Jogador.Usuario);
        Assert.Equal(Lado.Oposicao, res.Jogador.Lado);
    }

    [Fact]
    public async Task EntrarComSenhaErrada_FalhaGenerica()
    {
        var (servico, _) = NovoServico();
        await servico.Registrar("raio", "aSenha#123", "aSenha#123", Lado.Reino);

        var res = await servico.Entrar("raio", "senhaErrada#1");

        Assert.False(res.Sucesso);
        Assert.Null(res.Jogador);
        Assert.Null(res.ErroCampo);           // não revela por qual campo
        Assert.Equal("credenciais_invalidas", res.ErroCodigo);
        Assert.False(string.IsNullOrWhiteSpace(res.ErroMensagem));
    }

    [Fact]
    public async Task EntrarComUsuarioInexistente_MesmaFalhaDeSenhaErrada()
    {
        var (servico, _) = NovoServico();

        var res = await servico.Entrar("fantasma", "aSenha#123");

        // indistinguível da senha errada: mesmo campo null, mesmo código
        Assert.Equal("credenciais_invalidas", res.ErroCodigo);
        Assert.Null(res.ErroCampo);
    }
}
