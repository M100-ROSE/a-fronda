using Fronda.Core;

namespace Fronda;

/// <summary>Entrada do jogo. Toda a montagem vive nas classes de Core/UI/Scenes.</summary>
internal static class Program
{
    private static void Main()
    {
        using GameApp jogo = new();
        jogo.Run();
    }
}
