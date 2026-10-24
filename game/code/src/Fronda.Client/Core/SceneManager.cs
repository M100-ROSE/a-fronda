namespace Fronda.Core;

/// <summary>
/// Gerencia a cena ativa (uma por vez, estrutura simples para a leva 1).
/// Trocar() é corte direto: o gerente não compõe duas cenas com alpha.
/// A transição por dissolução discreta (leva 3, pós-login → BemVindoScene)
/// por isso acontece DENTRO das cenas: a antiga dissolve até o degrau 0
/// (tudo vira noite), a troca acontece imperceptível e a nova dissolve de
/// volta para cima. Degrade documentado: sem esse arranjo, a troca do
/// gerente seria mesmo um corte direto — que continua funcionando por baixo.
/// </summary>
public sealed class SceneManager
{
    public Scene? Atual { get; set; }

    public void Trocar(Scene nova)
    {
        Atual = nova;
    }
}
