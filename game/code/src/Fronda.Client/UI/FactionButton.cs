using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Fronda.Auth;
using Fronda.Font;
using Fronda.Rendering;

namespace Fronda.UI;

/// <summary>
/// Botão de facção conforme a spec §3.3 (oposição, primário da tela: borda 2px
/// vermelho permanente) e §3.4 (reino, escolha oposta: borda neutra 1px, a cor
/// azul vive na insígnia 8×8 — pois borda azul sobre o painel dá só 1,7:1).
/// Insígnia 8×8 cravada no TOPO CENTRAL do botão @ (76,2) rel.; hover/pressão
/// preenche o pano do lado e a insígnia vira marfim (nunca some dentro dele).
/// Regra anti-arco-íris: com um lado selecionado, o outro recua (borda neutra
/// 1px Borda e identidade só na insígnia).
/// Leva 3 (feedback): micro-nudge de 1px do lado escolhido (sem bounce) e
/// sombra embutida Fundo #120E10 no topo do reino pressionado (spec §3.4).
/// </summary>
public sealed class FactionButton : Button
{
    public Lado Lado { get; }
    private readonly ToggleGroup _grupo;

    public FactionButton(Lado lado, ToggleGroup grupo)
    {
        Lado = lado;
        _grupo = grupo;
    }

    private Color Acento => Lado == Lado.Oposicao ? Palette.AcentoOposicao : Palette.AcentoReino;
    private bool EhPrimario => Lado == Lado.Oposicao;

    private bool Selecionado => _grupo.Selecionado == Lado;
    private bool OutroEscolhido => _grupo.Selecionado.HasValue && _grupo.Selecionado.Value != Lado;

    public override void Desenhar(SpriteBatch lote, ShapeBatch formas, PixelFont fonte)
    {
        if (!Visivel) return;
        var e = EstadoAtual;
        bool selecionado = Selecionado;

        // ===== bandeiras por estado =====
        Color? borda;
        int espessura;

        if (e == Estado.Desabilitado)
        {
            borda = Palette.Borda;
            espessura = 1;
        }
        else if (EhPrimario && !OutroEscolhido && e != Estado.Hover)
        {
            // oposição em default OU selecionada (não recuado): borda 2px colorida
            borda = Acento;
            espessura = 2;
        }
        else
        {
            // reino (e oposição recuada): borda neutra 1px Borda
            borda = Palette.Borda;
            espessura = 1;
        }

        var r = Limites;

        // micro-nudge (leva 3): escolhido sobe 1px; o outro recua 1px (sem bounce)
        if (e != Estado.Desabilitado)
        {
            if (selecionado) r.Offset(0, -1);
            else if (OutroEscolhido) r.Offset(0, 1);
        }
        // pressionado do reino: desce 1px como sombra (§3.4, tem prioridade)
        if (e == Estado.Pressionado && !EhPrimario) r.Offset(0, 1);

        // ===== pintura por estado =====
        if (e == Estado.Desabilitado)
        {
            formas.RetanguloCheio(r, CapaSobrePainel(Palette.Painel));
        }
        else if (e == Estado.Pressionado)
        {
            // §3.3: oposição pressionada usa #7A1418 (2º degrau do pano vermelho);
            // §3.4: reino pressionado preenche o azul e descE 1px
            formas.RetanguloCheio(r, CapaSobrePainel(EhPrimario ? Palette.Erro : Acento));
            if (!EhPrimario) // sombra embutida no topo (§3.4)
                formas.Filete(r.X, r.Y, r.Width, 1, CapaSobrePainel(Palette.Fundo));
        }
        else if (e == Estado.Hover || selecionado)
        {
            // antecipação da escolha: pano do lado + insígnia marfim (§3.3/§3.4)
            formas.RetanguloCheio(r, CapaSobrePainel(Acento));
        }
        else
        {
            formas.RetanguloCheio(r, CapaSobrePainel(Palette.Painel));
        }

        if (borda is not null)
            formas.RetanguloContorno(r, espessura, CapaSobrePainel(borda.Value));

        // rótulo centrado (pos.Y = 365 num botão de 40 — reserva do ç, §2.2)
        var tamanhoTexto = fonte.MedirTexto(Texto, Escala);
        fonte.Desenhar(lote, Texto, new Point(
            r.X + (r.Width - tamanhoTexto.X) / 2,
            fonte.TopoBaseCentradoNoSlot(Texto, Escala, r.Y, r.Height)),
            CapaSobrePainel(e == Estado.Desabilitado ? Palette.TextoMudo : Palette.Texto), Escala);

        // insígnia 8×8 no topo central @ (76,2) rel. (§3.3/§3.4)
        Color corInsignia = e == Estado.Desabilitado ? Palette.TextoMudo
            : e == Estado.Normal && !selecionado ? Acento
            : Palette.Texto; // hover/pressionado/selecionado: marfim, nunca some
        formas.RetanguloCheio(new Rectangle(r.X + (r.Width - 8) / 2, r.Y + 2, 8, 8),
            CapaSobrePainel(corInsignia));

        if (Focado) DesenharContornoFoco(formas);
    }
}
