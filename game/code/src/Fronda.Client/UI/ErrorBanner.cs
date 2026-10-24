using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Fronda.Font;
using Fronda.Rendering;

namespace Fronda.UI;

/// <summary>
/// Selo de erro (spec §3.5): placa sólida #7A1418 de 328×28 + texto #F3EDE0
/// escala 3 — a cor Erro muda de papel: de tinta para placa (texto direto
/// sobre o painel daria 1,65:1). Ícone "!" é o glifo 5×7 @ escala 3 (15×21);
/// mensagem de 1 linha (cap 16 glifos; cai de escala se estourar).
/// Leva 3: a placa ENTRA em 2 degraus de dissolução discreta (50% por ~3
/// frames, depois cor inteira; §3.5 + §6 — sem alpha suave).
/// </summary>
public sealed class ErrorBanner : Widget
{
    private const int FramesPorDegrau = 3;

    public string Mensagem = "";

    /// <summary>Contador de frames desde o Mostrar — reinicia a cada exibição.</summary>
    private int _framesDesdeMostrar;

    public override bool PodeFocar => false;

    public void Mostrar(string mensagem)
    {
        Mensagem = mensagem;
        Visivel = true;
        _framesDesdeMostrar = 0; // recomeça a entrada em degraus
    }

    public void Esconder() => Visivel = false;

    public override void Desenhar(SpriteBatch lote, ShapeBatch formas, PixelFont fonte)
    {
        if (!Visivel) return;
        var r = Limites;

        // 2 degraus de entrada: 0..(FramesPorDegrau-1) metade da cor, depois inteira;
        // se a cena inteira estiver dissolvendo, o degrau mais baixo vence
        int degrauEntrada = _framesDesdeMostrar < FramesPorDegrau ? 2 : 4;
        int degrau = Math.Min(DegrauDissolucao, degrauEntrada);
        _framesDesdeMostrar++;
        // (conta no Desenhar: roda exatamente 1×/frame nesta cena — idem
        // MousePressionado de Button, estado de animação da própria instância)

        Color placa = Palette.DissolverPara(Palette.Erro, Palette.Painel, degrau);
        Color tinta = Palette.DissolverPara(Palette.Texto, Palette.Painel, degrau);

        formas.RetanguloCheio(r, placa);

        // ícone "!" = o glifo 5×7 na escala 3 (15×21), @ x+8, gap 8 até a mensagem (§3.5)
        fonte.Desenhar(lote, "!", new Point(r.X + 8, r.Y + 3), tinta, 3);

        // mensagem à direita, escala 3 (pos.Y = selo + 3: 3 de teto p/ acento,
        // 4 de piso); se estourar a largura útil, cai 2 e depois 1
        int larguraUtil = r.Width - (8 + 15 + 8) - 8;
        int escala = fonte.LarguraTexto(Mensagem, 3) <= larguraUtil ? 3
            : fonte.LarguraTexto(Mensagem, 2) <= larguraUtil ? 2 : 1;
        fonte.Desenhar(lote, Mensagem, new Point(r.X + 31, r.Y + 3), tinta, escala);
    }
}
