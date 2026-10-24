using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Fronda.Font;
using Fronda.Rendering;

namespace Fronda.UI;

/// <summary>
/// Selo de erro (spec §3.5): placa sólida #7A1418 + texto #F3EDE0 — a cor Erro
/// muda de papel: de tinta para placa (texto direto sobre o painel daria 1,65:1).
/// Ícone "!" pixelado 8×8 + texto; 24px por linha; se o texto estourar a
/// largura, cai para escala 1 (mensagem compacta).
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

        // ícone "!" 8×8 marfim: bastão 2×4 + ponto 2×2
        int ix = r.X + 8;
        int iy = r.Y + (r.Height - 8) / 2;
        formas.RetanguloCheio(new Rectangle(ix, iy, 2, 4), tinta);
        formas.RetanguloCheio(new Rectangle(ix, iy + 6, 2, 2), tinta);

        // texto centrado verticalmente; escala 2 se couber, senão escala 1
        int larguraUtil = r.Width - 24;
        int escala = fonte.LarguraTexto(Mensagem, 2) <= larguraUtil ? 2 : 1;
        var tamanho = fonte.MedirTexto(Mensagem, escala);
        fonte.Desenhar(lote, Mensagem,
            new Point(r.X + 20, r.Y + (r.Height - tamanho.Y) / 2), tinta, escala);
    }
}
