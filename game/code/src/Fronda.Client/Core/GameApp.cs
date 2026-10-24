using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Fronda.Font;
using Fronda.Rendering;

namespace Fronda.Core;

/// <summary>
/// Game principal: resolução lógica 960×540 (spec §2.1). A janela abre em
/// escala inteira — ×2 (1920×1080); se a altura não couber na área de trabalho,
/// cai para ×1 (960×540), nunca valor fracionário. O pixel inteiro é feito com
/// transform matrix de fator inteiro no SpriteBatch + PointClamp +
/// PreferMultiSampling = false; coordenadas lógicas são invioláveis e o mouse
/// chega à cena dividido pelo mesmo fator (§2.1: converter ÷2 na entrega).
/// Fonte px e formas são criadas aqui e entregues às cenas — nada estático
/// mutável global.
/// </summary>
public sealed class GameApp : Game
{
    private readonly GraphicsDeviceManager _graficos;
    private readonly SceneManager _cenas = new();

    /// <summary>Fator inteiro de escala UI da janela: 2 (1920×1080) ou 1 (fallback 960×540).</summary>
    private readonly int _escalaUi = 1;

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

        // escala inteira sancionada (spec §2.1): ×2 se a janela 1080p couber, senão ×1
        try
        {
            if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height >= 1080) _escalaUi = 2;
        }
        catch (Exception)
        {
            _escalaUi = 1; // sem medida de monitor conhecida: o inteiro para baixo
        }

        _graficos.PreferredBackBufferWidth = 960 * _escalaUi;
        _graficos.PreferredBackBufferHeight = 540 * _escalaUi;
    }

    protected override void Initialize()
    {
        Window.AllowUserResizing = false; // janela fixa: sem resize fracionário
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

        // mouse chega em pixels de janela (0..1920 na escala ×2): converte para o
        // espaço lógico (÷ fator inteiro) antes de entregar às cenas (spec §2.1)
        var bruto = Mouse.GetState();
        var mouse = _escalaUi == 1 ? bruto : new MouseState(
            bruto.X / _escalaUi, bruto.Y / _escalaUi, bruto.ScrollWheelValue,
            bruto.LeftButton, bruto.MiddleButton, bruto.RightButton,
            bruto.XButton1, bruto.XButton2);

        _cenas.Atual?.Atualizar(tempo, teclado, _tecladoAntigo, mouse, _mouseAntigo);

        _tecladoAntigo = teclado;
        _mouseAntigo = mouse;
        base.Update(tempo);
    }

    protected override void Draw(GameTime tempo)
    {
        GraphicsDevice.Clear(Palette.Fundo);
        if (_cenas.Atual is null) return;

        // um Begin/End por frame; transform matrix × fator inteiro mapeia as
        // coordenadas lógicas 960×540 para a janela; PointClamp = sem blur.
        var transformacao = Matrix.CreateScale(_escalaUi);
        _lote.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp,
            DepthStencilState.None, RasterizerState.CullCounterClockwise, null, transformacao);
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
