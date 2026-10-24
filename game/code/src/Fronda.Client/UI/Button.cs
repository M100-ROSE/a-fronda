using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Fronda.Font;
using Fronda.Rendering;

namespace Fronda.UI;

/// <summary>
/// Botão base: estados Default/Hover/Pressed/Disabled, desce 1px quando
/// pressionado (a 1px exposta acima lê como sombra "desce embutido" da spec),
/// texto centrado e outline de foco. Subclasses decorem por estado.
/// </summary>
public abstract class Button : Widget
{
    public string Texto = "";
    public int Escala = 2;
    public Action? AoClicar;

    /// <summary>Mouse desceu sobre o botão e ainda não subiu.</summary>
    public bool MousePressionado;

    public enum Estado
    {
        Normal,
        Hover,
        Pressionado,
        Desabilitado,
    }

    public Estado EstadoAtual
    {
        get
        {
            if (Desabilitado) return Estado.Desabilitado;
            if (MousePressionado) return Estado.Pressionado;
            if (Hover) return Estado.Hover;
            return Estado.Normal;
        }
    }

    public override void AtualizarEntrada(MouseState mouse, MouseState mouseAntigo)
    {
        Hover = PodeFocar && ContemPonto(mouse.Position);

        bool down = mouse.LeftButton == ButtonState.Pressed;
        if (down && Hover) MousePressionado = true;
        bool soltou = MousePressionado && !down;
        if (!down) MousePressionado = false;
        // soltou sobre o botão = clique (mesmo que a descida tenha começado fora)
        if (soltou && Hover) AoClicar?.Invoke();
    }

    public override bool ReceberTecla(Keys tecla)
    {
        if (Desabilitado) return false;
        if (tecla == Keys.Space)
        {
            AoClicar?.Invoke();
            return true;
        }
        return false;
    }

    public override void Desenhar(SpriteBatch lote, ShapeBatch formas, PixelFont fonte)
    {
        if (!Visivel) return;
        var e = EstadoAtual;

        // bloco desce 1px quando pressionado (o fundo do painel aparece como sombra)
        var r = Limites;
        if (e == Estado.Pressionado && DesceAoPressionar)
            r.Offset(0, 1);

        var fundo = CapaSobrePainel(CorFundo(e));
        formas.RetanguloCheio(r, fundo);

        var borda = CorBorda(e);
        if (borda is not null)
            formas.RetanguloContorno(r, EspessuraBorda(e), CapaSobrePainel(borda.Value));

        var tamanho = fonte.MedirTexto(Texto, Escala);
        fonte.Desenhar(lote, Texto, new Point(
            r.X + (r.Width - tamanho.X) / 2,
            r.Y + (r.Height - tamanho.Y) / 2), CapaSobrePainel(CorTexto(e)), Escala);

        if (Focado) DesenharContornoFoco(formas);
        Extra(lote, formas, fonte, r, e);
    }

    /// <summary>Engancho para insígnias/extra sobre o desenho padrão.</summary>
    protected virtual void Extra(SpriteBatch lote, ShapeBatch formas, PixelFont fonte, Rectangle r, Estado e) { }

    /// <summary>Espaço horizontal reservado à esquerda do texto (insígnias).</summary>
    protected virtual int EspacoEsquerdo(Estado e) => 0;

    protected virtual Color CorFundo(Estado e) => Palette.Painel;
    protected virtual Color? CorBorda(Estado e) => null;
    protected virtual int EspessuraBorda(Estado e) => 1;
    protected virtual Color CorTexto(Estado e) => Palette.Texto;
    protected virtual bool DesceAoPressionar => false;
}
