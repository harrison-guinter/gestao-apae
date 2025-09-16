using SistemaApae.Api.Models;

namespace SistemaApae.Api.Services;

/// <summary>
/// Serviço de email (implementação básica para desenvolvimento)
/// </summary>
public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Inicializa uma nova instância do EmailService
    /// </summary>
    /// <param name="logger">Logger para registro de eventos</param>
    /// <param name="configuration">Configuração da aplicação</param>
    public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }
    
    /// <summary>
    /// Envia email com nova senha gerada
    /// </summary>
    /// <param name="email">Email do destinatário</param>
    /// <param name="name">Nome do usuário</param>
    /// <param name="newPassword">Nova senha gerada</param>
    /// <returns>True se enviado com sucesso</returns>
    public async Task<bool> SendNewPasswordEmailAsync(string email, string name, string newPassword)
    {
        try
        {
            _logger.LogInformation("=== EMAIL COM NOVA SENHA ===");
            _logger.LogInformation("Para: {Email}", email);
            _logger.LogInformation("Nome: {Name}", name);
            _logger.LogInformation("Nova Senha: {NewPassword}", newPassword);
            _logger.LogInformation("=============================");

            // Simula envio de email
            await Task.Delay(100);
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar email com nova senha para: {Email}", email);
            return false;
        }
    }
}
