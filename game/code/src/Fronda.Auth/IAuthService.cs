namespace Fronda.Auth;

/// <summary>
/// Contrato front ↔ back para a leva 2 (implementação LOCAL: dentro do próprio
/// processo do jogo, persistência em arquivo JSON — a assinatura assíncrona
/// existe para permitir troca futura por rede sem mudar o cliente).
///
/// <para><c>Resultado Registrar(string usuario, string senha, string confirmaSenha, Lado lado)</c></para>
///   → POST /auth/registrar (futuro), corpo {usuario, senha, confirmaSenha, lado}.
///   Regras revalidadas no back-end (nunca confiar no front):
///   usuario 3–20 chars de a-z0-9_- (normalizado para minúsculas antes de
///   validar/persistir); senha mínimo 8; confirmaSenha igual à senha; lado
///   obrigatório (reino|oposicao). Erros: um por campo — {campo, codigo,
///   mensagem}, códigos curtos. Sucesso devolve o jogador criado.
///
/// <para><c>Resultado Entrar(string usuario, string senha)</c></para>
///   → POST /auth/entrar (futuro), corpo {usuario, senha}. Sucesso devolve o
///   jogador (id, usuario, lado); credenciais inválidas → falha genérica
///   {campo:null, codigo:"credenciais_invalidas"} — nunca revelar se o usuário
///   existe. "lado" NÃO é usado no login.
///
/// <para>Resposta de erro padronizada: {campo?, codigo, mensagem}. O front
/// mapeia campo → posição do selo de erro e exibe a mensagem PT-BR direto
/// (o código curto serve para contrato/testes, não para exibição).
/// Erros de formulário locais (campo vazio, senhas diferentes) não chamam o serviço.</para>
/// </summary>
public interface IAuthService
{
    Task<Resultado> Registrar(string usuario, string senha, string confirmaSenha, Lado lado);
    Task<Resultado> Entrar(string usuario, string senha);
}
