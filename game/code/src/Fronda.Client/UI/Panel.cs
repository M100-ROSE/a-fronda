using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Fronda.Font;
using Fronda.Rendering;

namespace Fronda.UI;

/// <summary>
/// Painel/card (spec §3.1): BG Painel + borda 1px Borda. Contorno externo de
/// 1px CARVÃO #1A1210 (spec §6, leva 3) — sancionado: a cor existe na paleta
/// oficial (inspo/paleta.md, vila revolucionária nº 10, uso "contorno") e o
/// briefing manda contorno de 1px em "carvão ou grafite". Dissolve para a noite
/// na entrada da cena (leva 3).
/// </summary>
public sealed class Panel : Widget
{
    public override bool PodeFocar => false; // foco mora nos filhos

    public override void Desenhar(SpriteBatch lote, ShapeBatch formas, PixelFont fonte)
    {
        // painel mora direto sobre a noite: dissolve na direção do Fundo
        formas.RetanguloCheio(Limites, CapaSobreFundo(Palette.Painel));
        formas.RetanguloContorno(Limites, 1, CapaSobreFundo(Palette.Borda));

        // contorno externo 1px carvão #1A1210 (sancionado pelo briefing)
        formas.RetanguloContorno(
            new Rectangle(Limites.X - 1, Limites.Y - 1, Limites.Width + 2, Limites.Height + 2),
            1, CapaSobreFundo(Palette.Carvao));
    }
}
