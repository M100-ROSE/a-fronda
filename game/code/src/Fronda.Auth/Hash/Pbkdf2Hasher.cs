using System.Security.Cryptography;

namespace Fronda.Auth.Hash;

/// <summary>
/// PBKDF2-HMAC-SHA256 via Rfc2898DeriveBytes: salt aleatório de 16 bytes por
/// conta, 150.000 iterações, chave de 32 bytes. hash/salt travel em base64.
/// Comparação em tempo constante (CryptographicOperations.FixedTimeEquals).
/// </summary>
public sealed class Pbkdf2Hasher : IHasher
{
    private const int Iteracoes = 150_000;
    private const int TamanhoSalt = 16;
    private const int TamanhoChave = 32;

    public (string Hash, string Salt) Gerar(string senha)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(TamanhoSalt);
        return (Convert.ToBase64String(Calcular(senha, salt)), Convert.ToBase64String(salt));
    }

    public bool Conferir(string senha, string hash, string salt)
    {
        byte[] saltBytes, hashEsperado;
        try
        {
            saltBytes = Convert.FromBase64String(salt);
            hashEsperado = Convert.FromBase64String(hash);
        }
        catch (FormatException)
        {
            return false; // dados corrompidos: nunca bate (e não vaza layout)
        }

        byte[] hashAtual = Calcular(senha, saltBytes);
        return hashAtual.Length == hashEsperado.Length
            && CryptographicOperations.FixedTimeEquals(hashAtual, hashEsperado);
    }

    private static byte[] Calcular(string senha, byte[] salt)
        => Rfc2898DeriveBytes.Pbkdf2(
            senha ?? string.Empty, salt, Iteracoes, HashAlgorithmName.SHA256, TamanhoChave);
}
