namespace Fronda.Auth;

/// <summary>Jogador devolvido pela autenticação.</summary>
public sealed class Jogador
{
    public required int Id { get; set; }
    public required string Usuario { get; set; }
    public required Lado Lado { get; set; }
}
