using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Fronda.Font;
using Fronda.Rendering;

namespace Fronda.Core;

/// <summary>
/// Game principal: backbuffer lógico 960×540, janela fixa (sem redimensionar),
/// PointClamp para preservar o contorno 1px. Fonte px e formas são criadas aqui
/// e entregues às cenas — nenhum estado estático mutável global.
/// </summary>
public sealed class GameApp : Game
{
    private readonly GraphicsDeviceManager _graficos;
    private readonly SceneManager _cenas = new();

    private SpriteBatch _lote = null!;
    private ShapeBatch _formas = null!;
    private PixelFont _fonte = null!;

    private KeyboardState _tecladoAntigo;
    private MouseState _mouseAntigo;

    public PixelFont Fonte => _fonte;

    /// <summary>Ponto único de troca de cena (leva 3: pós-login → BemVindoScene).</summary>
    public void TrocarCena(Scene nova) => _cenas.Trocar(nova);

    public GameApp()
    {
        _graficos = new GraphicsDeviceManager(this) { PreferMultiSampling = false };
        _graficos.PreferredBackBufferWidth = 960;
        _graficos.PreferredBackBufferHeight = 540;
    }

    protected override void Initialize()
    {
        Window.AllowUserResizing = false; // escala inteira: 1× (960×540)
        Window.Title = "a-fronda";
        IsMouseVisible = true;
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _lote = new SpriteBatch(GraphicsDevice);
        _formas = new ShapeBatch(_lote, GraphicsDevice);
        _fonte = new PixelFont(GraphicsDevice);
        _cenas.Trocar(new Scenes.LoginScene(this));
    }

    protected override void Update(GameTime tempo)
    {
        var teclado = Keyboard.GetState();
        var mouse = Mouse.GetState();

        _cenas.Atual?.Atualizar(tempo, teclado, _tecladoAntigo, mouse, _mouseAntigo);

        _tecladoAntigo = teclado;
        _mouseAntigo = mouse;
        base.Update(tempo);
    }

    protected override void Draw(GameTime tempo)
    {
        GraphicsDevice.Clear(Palette.Fundo);
        if (_cenas.Atual is null) return;

        // um Begin/End por frame; PointClamp = sem blur, pixel inteiro.
        _lote.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp,
            DepthStencilState.None, RasterizerState.CullCounterClockwise);
        try
        {
            _cenas.Atual.Desenhar(_lote, _formas);
        }
        finally
        {
            _lote.End();
        }
        base.Draw(tempo);
    }
}
