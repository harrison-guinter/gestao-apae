using System.Net;
using System.Net.Mail;
using SistemaApae.Api.Models;

namespace SistemaApae.Api.Services;

/// <summary>
/// Serviço de email usando System.Net.Mail
/// </summary>
public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IConfiguration _configuration;

    public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Envia email com nova senha gerada
    /// </summary>
    public async Task<bool> SendNewPasswordEmailAsync(string email, string name, string newPassword)
    {
        try
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var port = int.Parse(_configuration["EmailSettings:Port"] ?? "587");
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var senderName = _configuration["EmailSettings:SenderName"];
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];
            var enableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"] ?? "true");

            using var smtp = new SmtpClient(smtpServer)
            {
                Port = port,
                Credentials = new NetworkCredential(username, password),
                EnableSsl = enableSsl
            };

            using var message = new MailMessage();
            message.From = new MailAddress(senderEmail, senderName);
            message.To.Add(email);
            message.Subject = "Nova senha de acesso";
            message.Body = $"Olá {name},\n\nSua nova senha é: {newPassword}\nPor favor, altere após o primeiro login.";
            message.IsBodyHtml = false;

            await smtp.SendMailAsync(message);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar email para: {Email}", email);
            return false;
        }
    }
}
