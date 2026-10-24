namespace Fronda.Auth;

/// <summary>Resultado padronizado de autenticação: sucesso traz o jogador; falha traz campo, código e mensagem.</summary>
public sealed class Resultado
{
    public bool Sucesso { get; init; }
    public Jogador? Jogador { get; init; }

    /// <summary>"usuario" | "senha" | "confirmaSenha" | "lado" | null (erro geral).</summary>
    public string? ErroCampo { get; init; }

    /// <summary>Códigos curtos: usuario_curto, usuario_longo, usuario_invalido,
    /// usuario_em_uso, senha_curta, senha_diferente, lado_obrigatorio,
    /// credenciais_invalidas, backend_ausente.</summary>
    public string? ErroCodigo { get; init; }

    public string? ErroMensagem { get; init; }

    public static Resultado Ok(Jogador jogador) => new() { Sucesso = true, Jogador = jogador };

    public static Resultado Falha(string? campo, string codigo, string mensagem) => new()
    {
        Sucesso = false,
        ErroCampo = campo,
        ErroCodigo = codigo,
        ErroMensagem = mensagem,
    };
}
