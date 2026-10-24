namespace Fronda.Auth.Hash;

/// <summary>Contrato de hashing de senha: gera (hash, salt) e confere em tempo constante.</summary>
public interface IHasher
{
    /// <summary>Gera hash e salt (base64) para a senha; salt aleatório por chamada.</summary>
    (string Hash, string Salt) Gerar(string senha);

    /// <summary>Confere a senha contra hash/salt persistidos (base64) sem vazamento de tempo.</summary>
    bool Conferir(string senha, string hash, string salt);
}
