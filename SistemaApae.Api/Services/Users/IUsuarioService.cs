using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Users;

namespace SistemaApae.Api.Services.Users;

/// <summary>
/// Interface para serviço de usuários
/// </summary>
public interface IUsuarioService
{
    /// <summary>
    /// Busca um usuário por email
    /// </summary>
    Task<ApiResponse<UsuarioDto>> GetUserByEmail(string email);

    /// <summary>
    /// Busca um usuário por Id
    /// </summary>
    Task<ApiResponse<UsuarioDto>> GetUserById(Guid id);

    /// <summary>
    /// Lista todos os usuários
    /// </summary>
    Task<ApiResponse<IEnumerable<UsuarioDto>>> GetAllUsers();

    /// <summary>
    /// Cria um novo usuário
    /// </summary>
    Task<ApiResponse<UsuarioDto>> CreateUser(Usuario request);

    /// <summary>
    /// Atualiza um novo usuário existente
    /// </summary>
    Task<ApiResponse<UsuarioDto>> UpdateUser(Usuario request);
}
