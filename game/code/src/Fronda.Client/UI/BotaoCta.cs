using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Fronda.Auth;
using Fronda.Font;
using Fronda.Rendering;

namespace Fronda.UI;

/// <summary>
/// CTA principal (entrar / criar conta), estados da spec §3.6:
/// - modo entrar: neutro #3A3230, sem borda; hover/pressionado mantém BG (sombra).
/// - modo cadastrar: desligado até o lado ser escolhido (rótulo vira "escolha
///   seu lado"); com lado, herda o acento do lado escolhido.
/// - desabilitado: BG Painel + borda 1px Borda + texto mudo.
/// </summary>
public sealed class BotaoCta : Button
{
    public bool ModoCadastro { get; set; }

    private ToggleGroup? _grupo;
    public ToggleGroup? Grupo // definido pela cena no modo cadastrar
    {
        get => _grupo;
        set => _grupo = value;
    }

    private Color? AcentoAtual
    {
        get
        {
            if (!ModoCadastro || _grupo is null || !_grupo.Selecionado.HasValue) return null;
            return _grupo.Selecionado.Value == Lado.Oposicao
                ? Palette.AcentoOposicao
                : Palette.AcentoReino;
        }
    }

    protected override Color CorFundo(Estado e)
    {
        if (e == Estado.Desabilitado) return Palette.Painel;
        if (ModoCadastro)
        {
            var acento = AcentoAtual;
            if (acento is null) return Palette.Borda; // neutro do entrar (§3.6)
            if (acento == Palette.AcentoOposicao &&
                (e == Estado.Hover || e == Estado.Pressionado))
                return Palette.Erro; // degrau de sombra do pano vermelho (§3.6)
            return acento.Value;
        }
        return Palette.Borda; // modo entrar: neutro
    }

    protected override Color? CorBorda(Estado e) => e == Estado.Desabilitado ? Palette.Borda : null;

    protected override Color CorTexto(Estado e)
        => e == Estado.Desabilitado ? Palette.TextoMudo : Palette.Texto;

    protected override bool DesceAoPressionar => true;

    protected override void Extra(SpriteBatch lote, ShapeBatch formas, PixelFont fonte, Rectangle r, Estado e)
    {
        // sombra embutida: desce 1px ao pressionar + filete 1px Fundo no topo
        // (§3.6 "bloco desce 1px; filete 1px #120E10 no topo como sombra")
        if (e == Estado.Pressionado && !Desabilitado)
            formas.Filete(r.X, r.Y, r.Width, 1, CapaSobrePainel(Palette.Fundo));

        // hover sem acento (entrar, ou cadastrar ainda sem lado): outline Foco (§3.6)
        if (!Focado && Hover && AcentoAtual is null)
            DesenharContornoFoco(formas);
    }}
