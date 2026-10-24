using Fronda.Auth;
using Fronda.Auth.Armazenamento;
using Fronda.Auth.Hash;
using Xunit;

namespace Fronda.Auth.Tests;

/// <summary>Base com caminho de arquivo próprio por teste — NÃO usa o %APPDATA% real.</summary>
public abstract class TesteComArquivoTemporario
{
    protected string Caminho { get; }

    protected TesteComArquivoTemporario()
    {
        Caminho = Path.Combine(Path.GetTempPath(), $"fronda-tests-{Guid.NewGuid():N}", "usuarios.json");
    }

    protected JsonContasRepository NovoRepositorio() => new(Caminho);

    /// <summary>Serviço pronto para uso com seu próprio arquivo temporário.</summary>
    protected (AuthService Servico, JsonContasRepository Repositorio) NovoServico()
    {
        var repo = NovoRepositorio();
        return (new AuthService(new Pbkdf2Hasher(), repo), repo);
    }
}
