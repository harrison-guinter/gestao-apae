using System.ComponentModel.DataAnnotations;

namespace SistemaApae.Api.Models.Auth;

/// <summary>
/// Modelo para requisição de login
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Email do usuário
    /// </summary>
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email incorreto")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Senha do usuário
    /// </summary>
    [Required(ErrorMessage = "Senha é obrigatória")]
    [MinLength(6, ErrorMessage = "Senha incorreta")]
    public string Password { get; set; } = string.Empty;
}
