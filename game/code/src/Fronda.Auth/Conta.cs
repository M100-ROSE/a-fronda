namespace Fronda.Auth;

/// <summary>
/// Conta persistida no arquivo local de usuários. A senha NUNCA é gravada em
/// texto puro — só o hash PBKDF2 (base64) e o salt (base64) por conta.
/// O id é sequencial (1, 2, 3…), atribuído pelo repositório ao inserir.
/// </summary>
public sealed class Conta
{
    public int Id { get; set; }
    public required string Usuario { get; set; }
    public required string Hash { get; set; }
    public required string Salt { get; set; }
    public required Lado Lado { get; set; }
    public DateTime DataCriacao { get; set; }
}
