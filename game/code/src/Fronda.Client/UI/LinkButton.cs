using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Fronda.Font;
using Fronda.Rendering;

namespace Fronda.UI;

/// <summary>
/// Link de troca de modo (spec §3.7): texto pedra + underline 1px pedra;
/// hover/foco sobe para marfim; desabilitado durante request (sem underline).
/// </summary>
public sealed class LinkButton : Button
{
    protected override Color CorFundo(Estado e) => Palette.Painel; // transparente: painel já pinta
    protected override Color? CorBorda(Estado e) => null;
    protected override bool DesceAoPressionar => false;

    protected override Color CorTexto(Estado e)
        => Hover || Focado ? Palette.Texto : Palette.TextoMudo;

    public override void Desenhar(SpriteBatch lote, ShapeBatch formas, PixelFont fonte)
    {
        if (!Visivel) return;

        var tamanho = fonte.MedirTexto(Texto, Escala);
        int x = Limites.X + (Limites.Width - tamanho.X) / 2;
        int yBase = fonte.TopoBaseNoSlot(Texto, Escala, Limites.Y);

        fonte.Desenhar(lote, Texto, new Point(x, yBase), CapaSobrePainel(CorTexto(EstadoAtual)), Escala);

        // underline 1px na largura do texto, 2px abaixo da base ("linha 22 do
        // slot", spec §2.2/§3.7 — some quando desabilitado)
        if (!Desabilitado)
            formas.Filete(x, Limites.Y + 22, tamanho.X, 1, CapaSobrePainel(
                Hover || Focado ? Palette.Texto : Palette.TextoMudo));

        if (Focado) DesenharContornoFoco(formas);
    }
}
