using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Users;
using SistemaApae.Api.Models.Enums;

namespace SistemaApae.Api.Services;

/// <summary>
/// Interface para o serviço de autenticação
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Autentica um usuário com email e senha
    /// </summary>
    /// <param name="request">Dados de login</param>
    /// <returns>Resposta com token JWT e informações do usuário</returns>
    Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request);

    /// <summary>
    /// Envia email de recuperação de senha
    /// </summary>
    /// <param name="request">Email para recuperação</param>
    /// <returns>Resposta da operação</returns>
    Task<ApiResponse<object>> ForgotPasswordAsync(ForgotPasswordRequest request);

    /// <summary>
    /// Gera um token JWT para o usuário
    /// </summary>
    /// <param name="userId">ID do usuário</param>
    /// <param name="email">Email do usuário</param>
    /// <param name="roles">Roles do usuário</param>
    /// <returns>Token JWT</returns>
    string GenerateJwtToken(string userId, string email, List<string> roles);

    /// <summary>
    /// Gera uma senha aleatória segura
    /// </summary>
    /// <returns>Senha aleatória de 12 caracteres</returns>
    public string GenerateRandomPassword();
}
