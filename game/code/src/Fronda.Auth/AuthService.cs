using Fronda.Auth.Armazenamento;
using Fronda.Auth.Hash;

namespace Fronda.Auth;

/// <summary>
/// Autenticação local: revalida TUDO do lado de dentro (nunca confiar no front).
/// Coleção de contas em JSON, hash PBKDF2 por conta. Falhas de credenciais são
/// sempre genéricas — não revelam se o usuário existe. As tasks do contrato são
/// de fato síncronas por baixo (Task.FromResult: I/O local), mantendo a
/// assinatura assíncrona para a troca futura por rede.
/// </summary>
public sealed class AuthService : IAuthService
{
    private const int UsuarioMin = 3;
    private const int UsuarioMax = 20;
    private const int SenhaMin = 8;

    private readonly IHasher _hasher;
    private readonly IContasRepository _contas;

    public AuthService(IHasher hasher, IContasRepository contas)
    {
        _hasher = hasher;
        _contas = contas;
    }

    public Task<Resultado> Registrar(string usuario, string senha, string confirmaSenha, Lado lado)
        => Task.FromResult(RegistrarInterno(
            Normalizar(usuario), senha ?? "", confirmaSenha ?? "", lado));

    public Task<Resultado> Entrar(string usuario, string senha)
        => Task.FromResult(EntrarInterno(Normalizar(usuario), senha ?? ""));

    // ============================================================ registrar

    private Resultado RegistrarInterno(string usuario, string senha, string confirma, Lado lado)
    {
        if (usuario.Length < UsuarioMin)
            return FalhaUsuario("usuario_curto", "nome curto - 3 letras ou mais");
        if (usuario.Length > UsuarioMax)
            return FalhaUsuario("usuario_longo", "nome longo - até 20 letras");
        if (!FormatoUsuarioValido(usuario))
            return FalhaUsuario("usuario_invalido", "nome inválido - use a-z, 0-9, _ e -");
        if (senha.Length < SenhaMin)
            return Resultado.Falha("senha", "senha_curta", "senha curta - mínimo 8");
        if (!string.Equals(confirma, senha, StringComparison.Ordinal))
            return Resultado.Falha("confirmaSenha", "senha_diferente", "as senhas não coincidem");
        if (lado is not (Lado.Reino or Lado.Oposicao))
            return Resultado.Falha("lado", "lado_obrigatorio", "escolha seu lado");
        if (_contas.BuscarPorUsuario(usuario) is not null)
            return FalhaUsuario("usuario_em_uso", "este nome já está em uso");

        var (hash, salt) = _hasher.Gerar(senha);
        var conta = _contas.Adicionar(new Conta
        {
            Usuario = usuario,
            Hash = hash,
            Salt = salt,
            Lado = lado,
            DataCriacao = DateTime.UtcNow,
        });

        return Resultado.Ok(new Jogador { Id = conta.Id, Usuario = conta.Usuario, Lado = conta.Lado });
    }

    // ================================================================ entrar

    private Resultado EntrarInterno(string usuario, string senha)
    {
        // Falha única para usuário inexistente e senha errada: mesmo campo nulo,
        // mesmo código — não revela existência de contas.
        var conta = _contas.BuscarPorUsuario(usuario);
        if (conta is null || !_hasher.Conferir(senha, conta.Hash, conta.Salt))
            return Resultado.Falha(null, "credenciais_invalidas", "usuário ou senha incorretos");

        return Resultado.Ok(new Jogador { Id = conta.Id, Usuario = conta.Usuario, Lado = conta.Lado });
    }

    // ================================================================ helpers

    private static string Normalizar(string usuario)
        => (usuario ?? "").Trim().ToLowerInvariant();

    private static bool FormatoUsuarioValido(string usuario)
        => usuario.All(c => c is >= 'a' and <= 'z' or >= '0' and <= '9' or '_' or '-');

    private static Resultado FalhaUsuario(string codigo, string mensagem)
        => Resultado.Falha("usuario", codigo, mensagem);
}
