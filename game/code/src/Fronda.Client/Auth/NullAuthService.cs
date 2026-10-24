namespace Fronda.Auth;

/// <summary>
/// Stub da leva 1 (sempre falha com erro geral, código "backend_ausente").
/// Não é mais o serviço do jogo: a leva 2 compõe Fronda.Auth.AuthService no
/// cliente. Mantido aqui como fallback/testes — usa os mesmos tipos do
/// contrato, que agora moram na biblioteca Fronda.Auth (não há duplicação).
/// </summary>
public sealed class NullAuthService : IAuthService
{
    private static Resultado FalhaPadrao() => Resultado.Falha(
        campo: null,
        codigo: "backend_ausente",
        mensagem: "back-end não disponível nesta versão");

    public Task<Resultado> Registrar(string usuario, string senha, string confirmaSenha, Lado lado)
        => Task.FromResult(FalhaPadrao());

    public Task<Resultado> Entrar(string usuario, string senha)
        => Task.FromResult(FalhaPadrao());
}
