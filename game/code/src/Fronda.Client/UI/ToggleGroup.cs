using Fronda.Auth;

namespace Fronda.UI;

/// <summary>
/// Grupo de escolha de lado (não é widget: só estado). Regra da spec: exatamente
/// um lado por conta; trocar de modo limpa a escolha.
/// </summary>
public sealed class ToggleGroup
{
    public FactionButton BotaoOposicao { get; }
    public FactionButton BotaoReino { get; }

    public ToggleGroup()
    {
        BotaoOposicao = new FactionButton(Lado.Oposicao, this);
        BotaoReino = new FactionButton(Lado.Reino, this);
    }

    public Lado? Selecionado { get; private set; }

    public void Selecionar(Lado lado) => Selecionado = lado;

    public void Limpar() => Selecionado = null;

    public bool TemEscolha => Selecionado.HasValue;
}
