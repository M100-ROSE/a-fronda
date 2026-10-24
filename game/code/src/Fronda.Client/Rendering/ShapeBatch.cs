using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fronda.Rendering;

/// <summary>
/// Desenho primitivo via SpriteBatch + textura 1×1 branca: retângulos cheios,
/// contornos de 1px e segmentos. Nada de arte externa nesta leva.
/// </summary>
public sealed class ShapeBatch
{
    private readonly SpriteBatch _lote;
    private readonly Texture2D _pixel;

    public ShapeBatch(SpriteBatch lote, GraphicsDevice gd)
    {
        _lote = lote;
        _pixel = new Texture2D(gd, 1, 1);
        _pixel.SetData(new[] { Color.White });
    }

    public void RetanguloCheio(Rectangle r, Color cor)
    {
        if (r.Width <= 0 || r.Height <= 0) return;
        _lote.Draw(_pixel, r, cor);
    }

    /// <summary>Contorno dentro dos limites do retângulo (espessura px).</summary>
    public void RetanguloContorno(Rectangle r, int espessura, Color cor)
    {
        if (espessura <= 0 || r.Width - espessura * 2 <= 0) return;
        RetanguloCheio(new Rectangle(r.X, r.Y, r.Width, espessura), cor); // topo
        RetanguloCheio(new Rectangle(r.X, r.Bottom - espessura, r.Width, espessura), cor); // base
        RetanguloCheio(new Rectangle(r.X, r.Y + espessura, espessura, r.Height - espessura * 2), cor); // esq
        RetanguloCheio(new Rectangle(r.Right - espessura, r.Y + espessura, espessura, r.Height - espessura * 2), cor); // dir
    }

    /// <summary>Filete horizontal de espessura fixa.</summary>
    public void Filete(int x, int y, int largura, int espessura, Color cor)
        => RetanguloCheio(new Rectangle(x, y, largura, espessura), cor);

    /// <summary>Filete vertical de espessura fixa.</summary>
    public void FileteVertical(int x, int y, int altura, int espessura, Color cor)
        => RetanguloCheio(new Rectangle(x, y, espessura, altura), cor);
}
