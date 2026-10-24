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
/// Regra anti-arco-íris: com um lado selecionado, o outro recua (borda neutra
/// 1px Borda e identidade só na insígnia).
/// Leva 3 (feedback de hover/foque): micro-nudge de 1px do lado escolhido
/// (sem bounce) enquanto o outro recua 1px; filete inferior 2px intensificando
/// o lado escolhido — Erro #7A1418 (2º degrau do pano vermelho, spec §3.3) na
/// oposição e sombra embutida Fundo #120E10 no reino (idioma da spec §3.4).
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

    private Color Acento => Lado == LadoOposicao ? Palette.AcentoOposicao : Palette.AcentoReino;
    private bool EhPrimario => Lado == LadoOposicao;

    private static Lado LadoOposicao => Lado.Oposicao;

    private bool Selecionado => _grupo.Selecionado == Lado;
    private bool OutroEscolhido => _grupo.Selecionado.HasValue && _grupo.Selecionado.Value != Lado;

    public override void Desenhar(SpriteBatch lote, ShapeBatch formas, PixelFont fonte)
    {
        if (!Visivel) return;
        var e = EstadoAtual;
        bool selecionado = Selecionado;

        // ===== degraus por estado =====
        Color fundo = Palette.Painel;
        Color? borda = Palette.Borda;
        int espessura = 1;
        Color ins = Acento;
        bool desce = false;

        if (e == Estado.Desabilitado)
        {
            borda = Palette.Borda;
            ins = Palette.TextoMudo;
        }
        else if (e == Estado.Pressionado)
        {
            fundo = EhPrimario ? Palette.Erro : Acento; // §3.3/§3.4 pressionado
            borda = Acento;
            desce = !EhPrimario; // reino desce 1px; oposição usa 2º degrau
            ins = selecionado ? Palette.Texto : Acento; // §3.3: insígnia 8×8 Acento
        }
        else if (e == Estado.Hover || selecionado)
        {
            fundo = Acento; // preencher no hover = antecipação da escolha
            borda = Acento;
            ins = selecionado ? Palette.Texto : Acento;
        }
        else
        {
            // estado padrão: primário tem borda 2px colorida; reino neutro.
            // Recuo anti-arco-íris: se o OUTRO lado está escolhido, o primário
            // perde a borda colorida e fica neutro.
            if (EhPrimario && !OutroEscolhido)
            {
                borda = Acento;
                espessura = 2;
            }
        }

        var r = Limites;

        // micro-nudge (leva 3): escolhido sobe 1px; o outro recua 1px (sem bounce)
        if (e != Estado.Desabilitado)
        {
            if (selecionado) r.Offset(0, -1);
            else if (OutroEscolhido) r.Offset(0, 1);
        }
        // pressionado do reino: desce 1px como sombra (§3.4, tem prioridade)
        if (e == Estado.Pressionado && desce)
            r.Offset(0, 1);

        formas.RetanguloCheio(r, CapaSobrePainel(fundo));
        if (borda is not null)
            formas.RetanguloContorno(r, espessura, CapaSobrePainel(borda.Value));

        // filete intensificado do lado escolhido (leva 3): 2px na borda inferior
        if (selecionado && e != Estado.Desabilitado)
            formas.Filete(r.X, r.Bottom - 2, r.Width, 2,
                CapaSobrePainel(EhPrimario ? Palette.Erro : Palette.Fundo));

        // grupo centrado: insígnia 8×8 + gap 6 + rótulo
        int larguraTexto = fonte.LarguraTexto(Texto, Escala);
        int conteudo = 8 + 6 + larguraTexto;
        int x = r.X + (r.Width - conteudo) / 2;
        int yInsignia = r.Y + (r.Height - 8) / 2;

        var tamanhoTexto = fonte.MedirTexto(Texto, Escala);
        fonte.Desenhar(lote, Texto, new Point(
            x + 8 + 6,
            r.Y + (r.Height - tamanhoTexto.Y) / 2), CapaSobrePainel(Palette.Texto), Escala);

        formas.RetanguloCheio(new Rectangle(x, yInsignia, 8, 8), CapaSobrePainel(ins));

        if (Focado) DesenharContornoFoco(formas);
    }
}
