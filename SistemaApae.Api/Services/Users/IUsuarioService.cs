using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Users;

namespace SistemaApae.Api.Services.Users;

/// <summary>
/// Interface para serviço de usuários
/// </summary>
public interface IUsuarioService
{
    /// <summary>
    /// Lista usuários por filtros de pesquisa
    /// </summary>
    /// <returns> Lista de Usuario dos filtros de pesquisa </returns>
    Task<ApiResponse<IEnumerable<UsuarioDto>>> GetUserByFilters(UsuarioFiltroRequest filters);

    /// <summary>
    /// Busca um usuário por Id
    /// </summary>
    /// <returns> Usuario do id </returns>
    Task<ApiResponse<UsuarioDto>> GetUserById(Guid idUsuario);

    /// <summary>
    /// Lista todos os usuários
    /// </summary>
    /// <returns> Lista de Usuario </returns>
    Task<ApiResponse<IEnumerable<UsuarioDto>>> GetAllUsers();

    /// <summary>
    /// Cria um novo usuário
    /// </summary>
    /// <returns> Usuario criado </returns>
    Task<ApiResponse<UsuarioDto>> CreateUser(Usuario request);

    /// <summary>
    /// Atualiza um novo usuário existente
    /// </summary>
    /// <returns> Usuario atualizado </returns>
    Task<ApiResponse<UsuarioDto>> UpdateUser(Usuario request);

    /// <summary>
    /// Inativa um novo usuário existente
    /// </summary>
    /// <returns> Usuario inativado </returns>
    Task<ApiResponse<UsuarioDto>> DeleteUser(Guid idUsuario);
}
