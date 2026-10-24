namespace Fronda.Auth.Armazenamento;

/// <summary>Contrato de persistência de contas. Síncrono por baixo — I/O local de arquivo.</summary>
public interface IContasRepository
{
    /// <summary>Busca pela conta com o usuário exato (minúsculo), ou null.</summary>
    Conta? BuscarPorUsuario(string usuario);

    /// <summary>Insere a conta, atribui o id sequencial e devolve a conta salva.</summary>
    Conta Adicionar(Conta conta);
}
