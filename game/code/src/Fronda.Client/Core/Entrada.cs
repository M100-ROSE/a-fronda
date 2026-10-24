using Microsoft.Xna.Framework.Input;

namespace Fronda.Core;

/// <summary>
/// Utilidades de entrada: diff de teclas entre frames e mapeamento tecla→caractere.
/// O jogo só precisa dos conjuntos aceitos nos campos (a-z0-9_-), então o mapa é
/// propositalmente simples — acentos no teclado (dead keys) não viram chars aqui.
/// </summary>
public static class Entrada
{
    public static List<Keys> TeclasNovas(KeyboardState atual, KeyboardState antigo)
    {
        var novas = new List<Keys>();
        foreach (var tecla in atual.GetPressedKeys())
            if (!antigo.IsKeyDown(tecla))
                novas.Add(tecla);
        return novas;
    }

    public static char? TeclaParaChar(Keys tecla, bool shift)
    {
        // letras
        if (tecla >= Keys.A && tecla <= Keys.Z)
            return shift ? (char)('A' + (tecla - Keys.A)) : (char)('a' + (tecla - Keys.A));

        // números no teclado acima das letras
        if (tecla >= Keys.D0 && tecla <= Keys.D9)
            return (char)('0' + (tecla - Keys.D0));

        // números no bloco númerico
        if (tecla >= Keys.NumPad0 && tecla <= Keys.NumPad9)
            return (char)('0' + (tecla - Keys.NumPad0));

        return tecla switch
        {
            Keys.Space => ' ',
            Keys.OemMinus => shift ? '_' : '-',
            Keys.Subtract => '-',
            Keys.OemPeriod => '.',
            Keys.OemComma => ',',
            Keys.Decimal => '.',
            _ => null,
        };
    }
}
