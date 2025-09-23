namespace SistemaApae.Api.Models.Auth;

/// <summary>
/// Modelo para resposta de login
/// </summary>
public class LoginResponse
{
    /// <summary>
    /// Token JWT de autenticação
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Tipo do token
    /// </summary>
    public string TokenType { get; set; } = "Bearer";

    /// <summary>
    /// Tempo de expiração do token em segundos
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Informações do usuário autenticado
    /// </summary>
    public UserInfo User { get; set; } = new();
}

/// <summary>
/// Informações do usuário autenticado
/// </summary>
public class UserInfo
{
    /// <summary>
    /// ID único do usuário
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Nome do usuário
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Email do usuário
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Roles/permissões do usuário
    /// </summary>
    public List<string> Perfil { get; set; } = new();
}
