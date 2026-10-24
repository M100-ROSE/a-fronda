using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Fronda.Font;
using Fronda.Rendering;

namespace Fronda.UI;

/// <summary>
/// TextBox (spec §3.2): BG Fundo "recorte na parede", borda 1px Borda (hover
/// marfim, erro AcentoOposicao #C41E3A — nunca Erro como borda fina), foco
/// mantém a borda + outline externo Foco. Placeholder pedra. Senha em bullets.
/// Leva 3: caret piscando estilo pixel (30 frames aceso / 30 apagado, sem
/// interpolação; ao ganhar foco recomeça aceso; pára de contar na perda de
/// foco) e trem de 1px do input recém-errado (3 frames, sem bounce).
/// </summary>
public sealed class TextBox : Widget
{
    private const int FramesDoCaret = 30; // 30 aceso / 30 apagado (spec §6)
    private const int FramesDoTrem = 3;   // trem de 1px do erro: 2–3 frames

    public string Texto = "";
    public string Placeholder = "";
    public string Campo = ""; // chave p/ erros: "usuario"|"senha"|"confirmaSenha"
    public bool MascaraSenha;
    public int LimiteCaracteres = 32;
    public Func<char, bool>? FiltroCaractere;
    public Action? AoDigitar;

    public bool Erro;

    /// <summary>Frames desde que ganhou foco (conta só focado — pausa no blur).</summary>
    private int _framesComFoco;

    /// <summary>Frames restantes do trem de 1px disparado quando o erro chega.</summary>
    private int _framesDeTrem;

    /// <summary>Definido pela cena (substitui escrita direta em Erro): ao ligar,
    /// dispara o trem de 1px (leva 3).</summary>
    public void DefinirErro(bool erro)
    {
        if (erro && !Erro) _framesDeTrem = FramesDoTrem;
        Erro = erro;
    }

    public override void AtualizarEntrada(MouseState mouse, MouseState mouseAntigo)
    {
        bool estavaFocado = Focado;
        base.AtualizarEntrada(mouse, mouseAntigo);

        if (Focado && !estavaFocado) _framesComFoco = 0; // recomeça aceso
        if (Focado) _framesComFoco++;                    // pausa contando no blur
        if (_framesDeTrem > 0) _framesDeTrem--;          // erro chegou = 3 frames de trem
    }

    /// <summary>Caret piscando por degraus: 30 frames aceso, 30 apagado.</summary>
    private bool CaretAceso => _framesComFoco / FramesDoCaret % 2 == 0;

    public override void ReceberTexto(char c)
    {
        if (FiltroCaractere is not null && !FiltroCaractere(c)) return;
        if (Texto.Length >= LimiteCaracteres) return;
        Texto += c;
        AoDigitar?.Invoke();
    }

    public override bool ReceberTecla(Microsoft.Xna.Framework.Input.Keys tecla)
    {
        if (tecla == Microsoft.Xna.Framework.Input.Keys.Back && Texto.Length > 0)
        {
            Texto = Texto[..^1];
            AoDigitar?.Invoke();
            return true;
        }
        return false;
    }

    public override void Desenhar(SpriteBatch lote, ShapeBatch formas, PixelFont fonte)
    {
        // trem do erro: 1px para o lado por alguns frames, sem bounce (leva 3)
        var r = Limites;
        if (_framesDeTrem > 0) r.Offset(1, 0);

        // BG: Fundo (recorte); disabled muda para Painel + textos mudos
        Color bg = Desabilitado ? Palette.Painel : Palette.Fundo;
        Color corBorda = Desabilitado ? Palette.Borda
            : Erro ? Palette.AcentoOposicao           // erro (e erro+foco): §3.2
            : Hover && !Focado ? Palette.Texto        // hover borda marfim
            : Palette.Borda;

        formas.RetanguloCheio(r, CapaSobrePainel(bg));
        formas.RetanguloContorno(r, 1, CapaSobrePainel(corBorda));

        const int escala = 2;
        var corTxt = CapaSobrePainel(Desabilitado ? Palette.TextoMudo : Palette.Texto);

        string visivel = MascaraSenha ? new string('#', Texto.Length) : Texto;
        if (Texto.Length == 0)
        {
            if (!Focado) // placeholder pedra, some ao focar
                fonte.Desenhar(lote, Placeholder, new Point(r.X + 8, r.Y + 13),
                    CapaSobrePainel(Palette.TextoMudo), escala);
        }
        else if (MascaraSenha)
        {
            // bullet ▮ 8×14 por caractere, avança 12px (mesmo avanço de um glifo 2×)
            for (int i = 0; i < Texto.Length; i++)
            {
                int x = r.X + 8 + i * 12;
                formas.RetanguloCheio(new Rectangle(x, r.Y + 13, 8, 14), corTxt);
            }
        }
        else
        {
            fonte.Desenhar(lote, visivel, new Point(r.X + 8, r.Y + 13), corTxt, escala);
        }

        // caret piscando 2×16 marfim (leva 3: 30/30 frames)
        if (Focado && !Desabilitado && CaretAceso)
        {
            int xCaret = r.X + 8 + Texto.Length * 12;
            formas.RetanguloCheio(new Rectangle(xCaret, r.Y + 12, 2, 16), corTxt);
        }

        if (Focado) DesenharContornoFoco(formas);
    }
}
