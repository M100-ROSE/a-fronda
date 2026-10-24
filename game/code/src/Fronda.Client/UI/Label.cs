using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Fronda.Font;
using Fronda.Rendering;

namespace Fronda.UI;

/// <summary>Texto estático. O slot define o topo-âncora (spec §2.4, fórmula 3);
/// a cor vem da spec. Escala padrão da tela: corpo/labels 3.</summary>
public sealed class Label : Widget
{
    public string Texto = "";
    public Color Cor = Palette.Texto;
    public int Escala = 3;
    public bool Centrar = true;

    public override void Desenhar(SpriteBatch lote, ShapeBatch formas, PixelFont fonte)
    {
        if (!Visivel) return;
        var tamanho = fonte.MedirTexto(Texto, Escala);
        int x = Centrar
            ? Limites.X + (Limites.Width - tamanho.X) / 2
            : Limites.X;
        fonte.Desenhar(lote, Texto, new Point(x,
            fonte.TopoBaseNoSlot(Texto, Escala, Limites.Y)), CapaSobrePainel(Cor), Escala);
    }
}
