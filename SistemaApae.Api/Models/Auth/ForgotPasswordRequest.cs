using System.ComponentModel.DataAnnotations;

namespace SistemaApae.Api.Models.Auth;

/// <summary>
/// Modelo para requisição de recuperação de senha
/// </summary>
public class ForgotPasswordRequest
{
    /// <summary>
    /// Email do usuário para recuperação de senha
    /// </summary>
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
    public string Email { get; set; } = string.Empty;
}
