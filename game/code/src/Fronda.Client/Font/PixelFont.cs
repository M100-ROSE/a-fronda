using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fronda.Font;

/// <summary>
/// Fonte pixel 5×7 gerada proceduralmente em runtime — sem Content Pipeline,
/// sem .mgcb, sem SpriteFont. Cada glifo é uma malha de linhas ('X' pinta o
/// pixel, '.' deixa vazio — '#' também é aceito como pixel); acentos do PT-BR
/// são compostos sobrepondo diacrítico ao glifo-base. Avanço de 6px (5 de glifo
/// + 1 de espaço), escala inteira. Contrato de encaixe (spec §2.4):
/// `Desenhar` recebe pos.Y = TOPO DA BASE do glifo (com acento, o diacrítico
/// sobe acima por conta própria; com ç, desce abaixo da base).
/// </summary>
public sealed class PixelFont
{
    public const int LarguraGlifo = 5;
    public const int AlturaBase = 7;
    public const int AlturaComAcento = 10; // 3 linhas de acento no topo + 7 base
    public const int Avanco = 6;

    private readonly Dictionary<char, string[]> _glifos = new();
    private Texture2D? _pixel;

    public PixelFont(GraphicsDevice gd)
    {
        _pixel = new Texture2D(gd, 1, 1);
        _pixel.SetData(new[] { Color.White });
        RegistrarTudo();
    }

    // ---------------------------------------------------------------- medidas

    public Point MedirTexto(string texto, int escala)
        => new(texto.Length * Avanco * escala, AlturaBase * escala);

    public int LarguraTexto(string texto, int escala) => texto.Length * Avanco * escala;

    /// <summary>Altura de slot (§2.4, fórmula 2): 3·N se a linha tem acento,
    /// 7·N de base sempre, 2·N se tem ç.</summary>
    public int AlturaSlot(string texto, int escala)
        => ((TemAcento(texto) ? 3 : 0) + AlturaBase + (TemCedilha(texto) ? 2 : 0)) * escala;

    /// <summary>pos.Y a passar a Desenhar com o slot TOPO-ÂNCORA (§2.4, fórmula 3:
    /// topo do slot + 3·N se a linha tem acento; senão o próprio topo do slot).</summary>
    public int TopoBaseNoSlot(string texto, int escala, int topoSlot)
        => topoSlot + (TemAcento(texto) ? 3 * escala : 0);

    /// <summary>Idem TopoBaseNoSlot, com o bloco (acento + base + ç) centrado
    /// verticalmente no slot — regra dos botões/rótulos (tabs, facção, CTA).</summary>
    public int TopoBaseCentradoNoSlot(string texto, int escala, int topoSlot, int alturaSlot)
        => topoSlot + (alturaSlot - AlturaSlot(texto, escala)) / 2 + (TemAcento(texto) ? 3 * escala : 0);

    /// <summary>A linha tem algum diacrítico que sobe acima da base?</summary>
    public bool TemAcento(string texto) => texto.IndexOfAny(_comAcento) >= 0;

    /// <summary>A linha tem cedilha (ç / Ç) que desce abaixo da base?</summary>
    public bool TemCedilha(string texto)
        => texto.IndexOfAny(['ç', 'Ç']) >= 0;

    private static readonly char[] _comAcento =
        ['à', 'á', 'â', 'ã', 'è', 'é', 'ê', 'í', 'ò', 'ó', 'ô', 'õ', 'ù', 'ú', 'ü',
         'À', 'Á', 'Â', 'Ã', 'É', 'Ê', 'Í', 'Ó', 'Ô', 'Õ', 'Ú'];

    // ---------------------------------------------------------------- desenho

    /// <summary>
    /// y informado = topo da BASE do glifo; acentos sobem acima dele e a
    /// cedilha desce abaixo — por isso cada glifo desencaixa a partir da linha
    /// que corresponde à base (glifos de 10/12 linhas têm recuo de 3 no topo).
    /// Caractere desconhecido vira '?'.
    /// </summary>
    public void Desenhar(SpriteBatch lote, string texto, Point posicao, Color cor, int escala)
    {
        for (int i = 0; i < texto.Length; i++)
        {
            char c = texto[i];
            if (c == ' ') continue;
            if (!_glifos.TryGetValue(c, out var linhas))
            {
                c = '?';
                linhas = _glifos[c];
            }

            int xGlifo = posicao.X + i * Avanco * escala;
            int recuoAcento = (linhas.Length - AlturaBase) * escala; // linhas acima da base
            for (int linha = 0; linha < linhas.Length; linha++)
            {
                string r = linhas[linha];
                for (int col = 0; col < LarguraGlifo; col++)
                {
                    if (r[col] != 'X' && r[col] != '#') continue;
                    lote.Draw(_pixel,
                        new Rectangle(xGlifo + col * escala,
                            posicao.Y - recuoAcento + linha * escala, escala, escala),
                        cor);
                }
            }
        }
    }

    // ---------------------------------------------------------------- tabelas

    private void RegistrarTudo()
    {
        // ---- maiúsculas
        Maius(
            "A", ".XXX./X...X/X...X/XXXXX/X...X/X...X/X...X",
            "B", "XXXX./X...X/X...X/XXXX./X...X/X...X/XXXX.",
            "C", ".XXX./X...X/X..../X..../X..../X...X/.XXX.",
            "D", "XXXX./X...X/X...X/X...X/X...X/X...X/XXXX.",
            "E", "XXXXX/X..../X..../XXXX./X..../X..../XXXXX",
            "F", "XXXXX/X..../X..../XXXX./X..../X..../X....",
            "G", ".XXX./X...X/X..../X.XXX/X...X/X...X/.XXX.",
            "H", "X...X/X...X/X...X/XXXXX/X...X/X...X/X...X",
            "I", "XXXXX/..X../..X../..X../..X../..X../..X..",
            "J", "..XXX/...X./...X./...X./...X./X..X./.XX..",
            "K", "X...X/X..X./X.X../XX.../X.X../X..X./X...X",
            "L", "X..../X..../X..../X..../X..../X..../XXXXX",
            "M", "X...X/XX.XX/X.X.X/X.X.X/X...X/X...X/X...X",
            "N", "X...X/XX..X/X.X.X/X..XX/X...X/X...X/X...X",
            "O", ".XXX./X...X/X...X/X...X/X...X/X...X/.XXX.",
            "P", "XXXX./X...X/X...X/XXXX./X..../X..../X....",
            "Q", ".XXX./X...X/X...X/X...X/X.X.X/X..X./.XX.X",
            "R", "XXXX./X...X/X...X/XXXX./X.X../X..X./X...X",
            "S", ".XXXX/X..../X..../.XXX./....X/....X/XXXX.",
            "T", "XXXXX/..X../..X../..X../..X../..X../..X..",
            "U", "X...X/X...X/X...X/X...X/X...X/X...X/.XXX.",
            "V", "X...X/X...X/X...X/X...X/X...X/.X.X./..X..",
            "W", "X...X/X...X/X...X/X.X.X/X.X.X/X.X.X/.X.X.",
            "X", "X...X/X...X/.X.X./..X../.X.X./X...X/X...X",
            "Y", "X...X/X...X/.X.X./..X../..X../..X../..X..",
            "Z", "XXXXX/....X/...X./..X../.X.../X..../XXXXX");

        // ---- minúsculas
        Minus(
            "a", "...../...../.XXX./....X/.XXXX/X...X/.XXXX",
            "b", "X..../X..../X..../XXXX./X...X/X...X/XXXX.",
            "c", "...../...../.XXXX/X..../X..../X..../.XXXX",
            "d", "....X/....X/....X/.XXXX/X...X/X...X/.XXXX",
            "e", "...../...../.XXX./X...X/XXXXX/X..../.XXX.",
            "f", "..XX./.X.../X..../XXXX./X..../.X.../.X...",
            "g", "...../.XXXX/X...X/X...X/.XXXX/....X/.XXX.",
            "h", "X..../X..../X..../XXXX./X...X/X...X/X...X",
            "i", "..X../...../..X../..X../..X../..X../..X..",
            "j", "...X./...../...X./...X./...X./X..X./.XX..",
            "k", "X..../X..../X..X./X.X../XX.../X.X../X..X.",
            "l", "..X../..X../..X../..X../..X../..X../.XXX.",
            "m", "...../...../XX.X./X.X.X/X.X.X/X...X/X...X",
            "n", "...../...../XXXX./X...X/X...X/X...X/X...X",
            "o", "...../...../.XXX./X...X/X...X/X...X/.XXX.",
            "p", "...../XXXX./X...X/X...X/XXXX./X..../X....",
            "q", "...../.XXXX/X...X/X...X/.XXXX/....X/....X",
            "r", "...../...../X.XXX/XX.../X..../X..../X....",
            "s", "...../...../.XXXX/X..../.XXX./....X/XXXX.",
            "t", ".X.../X..../XXXX./.X.../.X.../.X.../..XX.",
            "u", "...../...../X...X/X...X/X...X/X...X/.XXX.",
            "v", "...../...../X...X/X...X/X...X/.X.X./..X..",
            "w", "...../...../X...X/X...X/X.X.X/X.X.X/.X.X.",
            "x", "...../...../X...X/.X.X./..X../.X.X./X...X",
            "y", "...../X...X/X...X/X...X/.XXXX/....X/.XXX.",
            "z", "...../...../XXXXX/...X./..X../.X.../XXXXX");

        // ---- dígitos
        Minus(
            "0", ".XXX./X...X/X..XX/X.X.X/XX..X/X...X/.XXX.",
            "1", "..X../.XX../..X../..X../..X../..X../XXXXX",
            "2", ".XXX./X...X/....X/...X./..X../.X.../XXXXX",
            "3", "XXXX./....X/....X/.XXX./....X/....X/XXXX.",
            "4", "...X./..XX./.X.X./X..X./XXXXX/...X./...X.",
            "5", "XXXXX/X..../XXXX./....X/....X/X...X/.XXX.",
            "6", "..XX./.X.../X..../XXXX./X...X/X...X/.XXX.",
            "7", "XXXXX/....X/...X./..X../.X.../.X.../.X...",
            "8", ".XXX./X...X/X...X/.XXX./X...X/X...X/.XXX.",
            "9", ".XXX./X...X/X...X/.XXXX/....X/...X./.XX..");

        // ---- pontuação básica
        Minus(
            "!", "..X../..X../..X../..X../..X../...../..X..",
            "\"", ".X.X./.X.X./...../...../...../...../.....",
            "#", ".X.X./XXXXX/.X.X./.X.X./XXXXX/.X.X./.....",
            "'", "..X../..X../...../...../...../...../.....",
            "(", "..XX./.X.../X..../X..../X..../.X.../..XX.",
            ")", "XX.../..X../...X./...X./...X./..X../XX...",
            "*", "...../X.X.X/.XXX./XXXXX/.XXX./X.X.X/.....",
            "+", "...../..X../..X../XXXXX/..X../..X../.....",
            ",", "...../...../...../...../..XX./..X../.X...",
            "-", "...../...../...../XXXXX/...../...../.....",
            ".", "...../...../...../...../...../..X../..X..",
            "/", "....X/....X/...X./..X../.X.../X..../X....",
            ":", "...../..X../..X../...../..X../..X../.....",
            ";", "...../..X../..X../...../..X../..X../.X...",
            "=", "...../...../XXXXX/...../XXXXX/...../.....",
            "?", ".XXX./X...X/....X/...X./..X../...../..X..",
            "_", "...../...../...../...../...../...../XXXXX",
            " ", "...../...../...../...../...../...../.....",
            // reticências dedicadas "…" (leva 3): três pontos 1×2 na base — cabe
            // em 1 avanço de glifo (em vez de "..." gastar 3)
            "…", "...../...../...../...../...../X.X.X/X.X.X");

        // ---- acentuação PT-BR: glifo-base + diacrítico sobreposto, mesma fonte.
        // tipos: 0 grave, 1 agudo, 2 circunflexo, 3 til, 4 trema, 5 cedilha
        (char, char, int)[] tabela =
        {
            ('à', 'a', 0), ('á', 'a', 1), ('â', 'a', 2), ('ã', 'a', 3),
            ('ç', 'c', 5),
            ('è', 'e', 0), ('é', 'e', 1), ('ê', 'e', 2),
            ('í', 'i', 1),
            ('ò', 'o', 0), ('ó', 'o', 1), ('ô', 'o', 2), ('õ', 'o', 3),
            ('ù', 'u', 0), ('ú', 'u', 1), ('ü', 'u', 4),
            ('À', 'A', 0), ('Á', 'A', 1), ('Â', 'A', 2), ('Ã', 'A', 3),
            ('Ç', 'C', 5),
            ('É', 'E', 1), ('Ê', 'E', 2), ('Í', 'I', 1),
            ('Ó', 'O', 1), ('Ô', 'O', 2), ('Õ', 'O', 3), ('Ú', 'U', 1),
        };
        foreach (var (letra, baseChar, tipo) in tabela)
        {
            var linhas = new List<string>(3 + AlturaBase + 2);
            if (tipo == 5)
            {
                linhas.Add(".....");
                linhas.Add(".....");
                linhas.Add(".....");
                linhas.AddRange(_glifos[baseChar]);
                linhas.Add(".X..."); // cedilha abaixo da base (2 linhas em gancho)
                linhas.Add("..XX.");
            }
            else
            {
                linhas.AddRange(Diacritico(tipo)); // 3 linhas no topo
                linhas.AddRange(_glifos[baseChar]);
            }
            _glifos[letra] = linhas.ToArray();
        }
    }

    /// <summary>Registra um glifo de cada par (chave, padrão de 7 linhas × 5 cols).</summary>
    private void Minus(params string[] pares) => RegistrarPares(pares);

    /// <summary>Idem Minus — separado só para leitura das tabelas.</summary>
    private void Maius(params string[] pares) => RegistrarPares(pares);

    private void RegistrarPares(string[] pares)
    {
        for (int i = 0; i + 1 < pares.Length; i += 2)
        {
            char chave = pares[i] is { Length: > 0 } k ? k[0] : '\0';
            var linhas = new List<string>(pares[i + 1].Split('/'));
            // garante altura fixa de 7 linhas (completa com linha vazia)
            while (linhas.Count < AlturaBase) linhas.Add(".....");
            // e largura fixa de 5 colunas (linhas curtas viram vazio à direita)
            for (int j = 0; j < linhas.Count; j++)
                linhas[j] = linhas[j].Length < LarguraGlifo
                    ? linhas[j].PadRight(LarguraGlifo, '.') : linhas[j];
            _glifos[chave] = linhas.ToArray();
        }
    }

    private static string[] Diacritico(int tipo) => tipo switch
    {
        0 => new[] { ".....", "...X.", "..X.." }, // grave (desce à esquerda)
        1 => new[] { ".....", ".X...", "..X.." }, // agudo (desce à direita)
        2 => new[] { ".....", "..X..", ".X.X." }, // circunflexo
        3 => new[] { ".....", ".X.X.", "X.X.X" }, // til
        4 => new[] { "X...X", ".....", "....." }, // trema
        _ => new[] { ".....", ".....", "....." },
    };
}
