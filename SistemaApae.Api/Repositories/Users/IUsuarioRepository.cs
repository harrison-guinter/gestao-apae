using SistemaApae.Api.Models.Users;

namespace SistemaApae.Api.Repositories.Users;

/// <summary>
/// Interface para repositório de usuários
/// </summary>
public interface IUsuarioRepository
{
    /// <summary>
    /// Busca um usuário por email
    /// </summary>
    /// <param name="email">Email do usuário</param>
    /// <returns>Usuário encontrado ou null</returns>
    Task<Usuario?> GetByEmailAsync(string email);

    /// <summary>
    /// Busca um usuário por Id
    /// </summary>
    /// <param name="id">ID do usuário</param>
    /// <returns>Usuário encontrado ou null</returns>
    Task<Usuario?> GetByIdAsync(Guid id);

    /// <summary>
    /// Lista todos os usuários
    /// </summary>
    /// <returns>Lista de usuários</returns>
    Task<IEnumerable<Usuario>> GetAllAsync();

    /// <summary>
    /// Cria um novo usuário
    /// </summary>
    /// <param name="usuario">Dados do usuário</param>
    /// <returns>Usuário criado</returns>
    Task<Usuario> CreateAsync(Usuario usuario);

    /// <summary>
    /// Atualiza um usuário existente
    /// </summary>
    /// <param name="usuario">Dados do usuário</param>
    /// <returns>Usuário atualizado</returns>
    Task<Usuario> UpdateAsync(Usuario usuario);

    /// <summary>
    /// Remove um usuário
    /// </summary>
    /// <param name="id">ID do usuário</param>
    /// <returns>True se removido com sucesso</returns>
    Task<bool> DeleteAsync(Guid id);
}
