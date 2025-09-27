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
    /// <returns> Usuario do email ou nulo </returns>
    Task<Usuario?> GetByEmailAsync(string email);

    /// <summary>
    /// Lista usuários por filtros de pesquisa
    /// </summary>
    /// <returns> Lista de Usuario dos filtros de pesquisa </returns>
    Task<IEnumerable<Usuario>> GetByFiltersAsync(UsuarioFiltroRequest filters);

    /// <summary>
    /// Busca um usuário por id
    /// </summary>
    /// <param name="idUser">ID do usuário</param>
    /// <returns> Usuario do id ou nulo </returns>
    Task<Usuario?> GetByIdAsync(Guid idUser);

    /// <summary>
    /// Lista todos os usuários
    /// </summary>
    /// <returns> Lista de Usuario </returns>
    Task<IEnumerable<Usuario>> GetAllAsync();

    /// <summary>
    /// Cria um novo usuário
    /// </summary>
    /// <param name="user">Dados do usuário</param>
    /// <returns> Usuario criado </returns>
    Task<Usuario> CreateAsync(Usuario user);

    /// <summary>
    /// Atualiza um usuário existente
    /// </summary>
    /// <param name="user">Dados do usuário</param>
    /// <returns> Usuario atualizado </returns>
    Task<Usuario> UpdateAsync(Usuario user);
}
