using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Fronda.Font;
using Fronda.Rendering;

namespace Fronda.UI;

/// <summary>
/// Base de todo widget: retângulo de limites, estado de foco/hover, hierarquia
/// (pai/filhos incluídos na ordem natural) e desenho. Nada aqui é estático.
/// </summary>
public abstract class Widget
{
    public Rectangle Limites;
    public bool Focado;
    public bool Hover;
    public bool Desabilitado;
    public bool Visivel = true;

    /// <summary>
    /// Degrau de dissolução do widget nas transições pixel-art da leva 3:
    /// 0 = pintado na cor do fundo (invisível), 4 (default) = cor inteira.
    /// Estado de animação por instância — nada estático mutável.
    /// </summary>
    public int DegrauDissolucao = 4;

    private Widget? _pai;
    private readonly List<Widget> _filhos = new();
    public IReadOnlyList<Widget> Filhos => _filhos;

    public Widget? Pai => _pai;

    /// <summary>Adiciona filho (a ordem de inserção define a ordem de foco).</summary>
    public void Adicionar(Widget filho)
    {
        filho._pai = this;
        _filhos.Add(filho);
    }

    public virtual bool PodeFocar => Visivel && !Desabilitado;

    public bool ContemPonto(Point p) => Limites.Contains(p);

    /// <summary>Por frame: atualiza hover e estados dependentes do mouse.</summary>
    public virtual void AtualizarEntrada(MouseState mouse, MouseState mouseAntigo)
    {
        Hover = PodeFocar && ContemPonto(mouse.Position);
    }

    public abstract void Desenhar(SpriteBatch lote, ShapeBatch formas, PixelFont fonte);

    /// <summary>Caractere digitado enquanto este widget está focado.</summary>
    public virtual void ReceberTexto(char c) { }

    /// <summary>Tecla especial (Backspace etc.) enquanto este widget está focado.</summary>
    public virtual bool ReceberTecla(Keys tecla) => false;

    /// <summary>
    /// Capa de dissolução (leva 3): mistura a cor em degraus discretos para
    /// `alvoFundo` conforme o degrau atual deste widget. Quem pinta sobre o
    /// painel usa CapaSobrePainel; quem pinta direto sobre a noite, CapaSobreFundo.
    /// </summary>
    protected Color Capa(Color cor, Color alvoFundo)
        => Palette.DissolverPara(cor, alvoFundo, DegrauDissolucao);

    protected Color CapaSobrePainel(Color cor) => Capa(cor, Palette.Painel);

    protected Color CapaSobreFundo(Color cor) => Capa(cor, Palette.Fundo);

    /// <summary>Outline de foco: 1px Foco, offset externo +1px (não substitui a borda).</summary>
    protected void DesenharContornoFoco(ShapeBatch formas)
    {
        formas.RetanguloContorno(
            new Rectangle(Limites.X - 2, Limites.Y - 2, Limites.Width + 4, Limites.Height + 4),
            1, CapaSobrePainel(Palette.Foco));
    }
}
