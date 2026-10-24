using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fronda.Rendering;

/// <summary>
/// Paleta de login/cadastro (inspo/paleta.md, seção "Login / cadastro").
/// Nenhum hex fora daqui entra na tela — qualquer exceção fica marcada como PENDENTE.
/// </summary>
public static class Palette
{
    public static readonly Color Fundo = new(0x12, 0x0E, 0x10); // canvas, noite
    public static readonly Color Painel = new(0x1C, 0x16, 0x14); // card, formulário
    public static readonly Color Borda = new(0x3A, 0x32, 0x30); // input, divisor, desligado
    public static readonly Color Texto = new(0xF3, 0xED, 0xE0); // labels, títulos (marfim)
    public static readonly Color TextoMudo = new(0x9A, 0x95, 0x8C); // placeholder, ajuda (pedra)
    public static readonly Color AcentoOposicao = new(0xC4, 0x1E, 0x3A); // CTA / lado oposição
    public static readonly Color AcentoReino = new(0x1E, 0x3A, 0x8A); // CTA / lado reino
    public static readonly Color Ouro = new(0xC9, 0xA2, 0x27); // marca, detalhe do logo
    public static readonly Color Erro = new(0x7A, 0x14, 0x18); // validação (placa, não tinta)
    public static readonly Color Foco = new(0xE8, 0xDC, 0xC8); // outline de acessibilidade

    // Carvão #1A1210 — APROVADO: existe na paleta oficial (inspo/paleta.md, vila
    // revolucionária da oposição, nº 10 — uso "contorno, noite, silhueta") e o
    // briefing sanciona contorno de 1px em "carvão ou grafite". Uso aqui:
    // contorno externo 1px dos painéis/módulos de UI (spec §6, leva 3).
    public static readonly Color Carvao = new(0x1A, 0x12, 0x10);

    /// <summary>
    /// Dissolução discreta das transições da leva 3 (entrada de cena, pós-login,
    /// erro): o lote roda com BlendState.Opaque, então alpha NÃO tem efeito — o
    /// "fade" pixel-art é feito em degraus de COR: mistura discreta da cor para
    /// a cor de fundo sobre a qual o elemento pinta (Fundo p/ a cena, Painel p/
    /// o que mora dentro do painel). Sem interpolação suave: degrau 0 = cor do
    /// fundo (invisível), degrau `degraus` = cor inteira. Puro: sem estado.
    /// </summary>
    public static Color DissolverPara(Color cor, Color alvoFundo, int degrau, int degraus = 4)
    {
        if (degrau >= degraus) return cor;
        if (degrau <= 0) return alvoFundo;
        return Color.Lerp(alvoFundo, cor, (float)degrau / degraus);
    }
}
