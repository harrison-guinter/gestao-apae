namespace SistemaApae.Api.Models.Enums;

/// <summary>
/// Enum para motivo do envio de email
/// </summary>
public enum EmailReasonEnum
{
    /// <summary>
    /// Criação de usuário
    /// </summary>
    CreateUser = 1,

    /// <summary>
    /// Usuário esqueceu a senha
    /// </summary>
    ForgotPassword = 2
}
