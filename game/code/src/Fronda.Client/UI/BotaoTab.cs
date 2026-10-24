using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Fronda.Font;
using Fronda.Rendering;

namespace Fronda.UI;

/// <summary>
/// Alternância entrar/cadastrar (spec §2.3): ativa = texto marfim + filete
/// inferior 2px de Ouro; inativa = pedra (texto sobe no hover); foco = outline.
/// </summary>
public sealed class BotaoTab : Button
{
    public bool Ativa { get; set; }

    protected override Color CorFundo(Estado e) => Palette.Painel;

    protected override Color? CorBorda(Estado e) => null;
    protected override Color CorTexto(Estado e)
        => Ativa || e != Estado.Normal ? Palette.Texto : Palette.TextoMudo;

    protected override bool DesceAoPressionar => false;

    protected override void Extra(SpriteBatch lote, ShapeBatch formas, PixelFont fonte, Rectangle r, Button.Estado e)
    {
        if (Ativa)
        {
            // filete inferior 2px Ouro (filete de marca da tab ativa) — dissolve com a cena
            formas.Filete(r.X, r.Bottom - 2, r.Width, 2, CapaSobrePainel(Palette.Ouro));
        }
    }
}
