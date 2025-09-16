namespace SistemaApae.Api.Services;

/// <summary>
/// Interface para serviço de email
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Envia email com nova senha gerada
    /// </summary>
    /// <param name="email">Email do destinatário</param>
    /// <param name="name">Nome do usuário</param>
    /// <param name="newPassword">Nova senha gerada</param>
    /// <returns>True se enviado com sucesso</returns>
    Task<bool> SendNewPasswordEmailAsync(string email, string name, string newPassword);
}
