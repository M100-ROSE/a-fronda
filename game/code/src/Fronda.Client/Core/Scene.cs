using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Fronda.Rendering;

namespace Fronda.Core;

/// <summary>Cena abstrata: recebe a entrada bruta de cada frame e se desenha.</summary>
public abstract class Scene
{
    protected readonly GameApp Jogo;

    protected Scene(GameApp jogo) => Jogo = jogo;

    public abstract void Atualizar(
        GameTime tempo,
        KeyboardState teclado, KeyboardState tecladoAntigo,
        MouseState mouse, MouseState mouseAntigo);

    public abstract void Desenhar(SpriteBatch lote, ShapeBatch formas);
}
