using System.Globalization;
using System.Net;
using System.Net.Mail;
using SistemaApae.Api.Models;
using SistemaApae.Api.Models.Enums;

namespace SistemaApae.Api.Services;

/// <summary>
/// Serviço de email usando System.Net.Mail
/// </summary>
public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IConfiguration _configuration;
	
	// Configs em cache
	private readonly string? _smtpServer;
	private readonly int _smtpPort;
	private readonly string? _senderEmail;
	private readonly string? _senderName;
	private readonly string? _username;
	private readonly string? _password;
	private readonly bool _enableSsl;

    public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;

		// Carrega configurações: primeiro variáveis de ambiente, depois appsettings, e loga a origem
		var envSmtp = Environment.GetEnvironmentVariable("SMTP_SERVER_EMAIL");
		_smtpServer = envSmtp ?? _configuration["EmailSettings:SmtpServer"];
		_logger.LogInformation("Email config: SMTP server source={Source}, value={Value}", envSmtp is not null ? "ENV" : "APPSETTINGS", _smtpServer);

		var envPort = Environment.GetEnvironmentVariable("PORT_EMAIL");
		var portStr = envPort ?? _configuration["EmailSettings:Port"] ?? "587";
		_smtpPort = int.TryParse(portStr, out var parsedPort) ? parsedPort : 587;
		_logger.LogInformation("Email config: SMTP port source={Source}, value={Value}", envPort is not null ? "ENV" : "APPSETTINGS", _smtpPort);

		var envSenderEmail = Environment.GetEnvironmentVariable("SENDER_EMAIL");
		_senderEmail = envSenderEmail ?? _configuration["EmailSettings:SenderEmail"];
		_logger.LogInformation("Email config: SenderEmail source={Source}, value={Value}", envSenderEmail is not null ? "ENV" : "APPSETTINGS", _senderEmail);

		var envSenderName = Environment.GetEnvironmentVariable("SENDER_NAME_EMAIL");
		_senderName = envSenderName ?? _configuration["EmailSettings:SenderName"];
		_logger.LogInformation("Email config: SenderName source={Source}, value={Value}", envSenderName is not null ? "ENV" : "APPSETTINGS", _senderName);

		var envUsername = Environment.GetEnvironmentVariable("USERNAME_EMAIL");
		_username = envUsername ?? _configuration["EmailSettings:Username"] ?? _senderEmail;
		_logger.LogInformation("Email config: Username source={Source}, value={Value}", envUsername is not null ? "ENV" : (_configuration["EmailSettings:Username"] is not null ? "APPSETTINGS" : "FALLBACK_SENDER_EMAIL"), _username);

		var envPassword = Environment.GetEnvironmentVariable("PASSWORD_EMAIL");
		_password = envPassword ?? _configuration["EmailSettings:Password"];
		_logger.LogInformation("Email config: Password source={Source}, set={Set}", envPassword is not null ? "ENV" : (_configuration["EmailSettings:Password"] is not null ? "APPSETTINGS" : "NONE"), string.IsNullOrEmpty(_password) ? "no" : "yes");

		var envEnableSsl = Environment.GetEnvironmentVariable("USE_SSL_EMAIL");
		var enableSslStr = envEnableSsl ?? _configuration["EmailSettings:EnableSsl"];
		_enableSsl = true;
		if (bool.TryParse(enableSslStr, out var parsedEnableSsl)) _enableSsl = parsedEnableSsl;
		_logger.LogInformation("Email config: EnableSsl source={Source}, value={Value}", envEnableSsl is not null ? "ENV" : (_configuration["EmailSettings:EnableSsl"] is not null ? "APPSETTINGS" : "DEFAULT_TRUE"), _enableSsl);
    }

    /// <summary>
    /// Envia email
    /// </summary>
    public async Task<bool> SendEmailAsync(string email, string name, string newPassword, EmailReasonEnum emailReason)
    {
        try
        {

			using var smtp = new SmtpClient(_smtpServer)
            {
				Port = _smtpPort,
				Credentials = new NetworkCredential(_username, _password),
				EnableSsl = _enableSsl
            };

            using var mail = new MailMessage();
			mail.From = new MailAddress(_senderEmail, _senderName);
            mail.To.Add(email);

            var message = string.Empty;
            switch (emailReason)
            {
                case EmailReasonEnum.CreateUser:
                    mail.Subject = "Bem-vindo(a) ao Sistema Apae!";
                    message = "Sua conta no Sistema Apae foi criada com sucesso.";

                    break;

                case EmailReasonEnum.ForgotPassword:
                    mail.Subject = "Redefinição de Senha - Sistema Apae";
                    message = "Recebemos uma solicitação para redefinir a senha da sua conta no Sistema Apae.";
                    break;
            }

            mail.Body = FillContent(name, email, newPassword, message);
            mail.IsBodyHtml = true;

            await smtp.SendMailAsync(mail);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar email para: {Email}", email);
            return false;
        }
    }

    private static string FillContent(string name, string email, string newPassword, string message)
    {
        string date = @$"{DateTime.Now.Day} de {DateTime.Now.ToString("MMMM", new CultureInfo("pt-br"))} de {DateTime.Now.Year}";

        string link = "link do sistema apae";

        string content = $@"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
                            <html xmlns='http://www.w3.org/1999/xhtml'>
                              <head>
                                <meta http-equiv='Content-Type' content='text/html; charset=UTF-8' />
                                <link
                                  href='https://fonts.googleapis.com/css2?family=Poppins:wght@100&display=swap'
                                  rel='stylesheet'
                                />
                              </head>
                              <body>
                                <style>
                                  @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@100&display=swap');
                                </style>
                                <div
                                  style='
                                    width: 95%;
                                    color: #262626;
                                    font-family: 'Poppins', Sans-serif;
                                    text-align: justify;
                                    line-height: 23px;
                                    margin: 0 auto;
                                  '
                                >
                                  </div>
                                  <div
                                    style='
                                      color: white;
                                      background-color: #003087;
                                      width: 100%;
                                      height: 30px;
                                    '
                                  >
                                    <div style='display: inline-table; width: 100%'>
                                      <div style='display: table-cell'></div>
                                      <div style='display: table-cell'>
                                        <div style='padding: 5px; padding-right: 15px; text-align: right'>
                                          {date}
                                        </div>
                                      </div>
                                    </div>
                                  </div>
                                  <div style='display: 'flex'; flex-direction: 'column'; gap: 0px; width: 100%'>
                                    <p>Olá {name},</p>

                                    <p>{message}</p>
                                    <p>Abaixo estão suas informações de acesso:</p>
                                    <div>
                                      <p style='margin: 0'>
                                        <span style='font-weight: 800'>Login:</span>
                                        {email}
                                      </p>
                                      <p style='margin: 0'>
                                        <span style='font-weight: 800'>Senha:</span>
                                        {newPassword}
                                      </p>
                                    </div>

                                    <p>Caso tenha qualquer dúvida, por favor, entre em contato.</p>
                                  </div>

                                  <div style='font-size: 12px'>
                                    <p style='text-align: right; color: #aaa; margin: 0'>
                                      Esta é uma mensagem automática, favor não responder.
                                    </p>
                                  </div>
                                  <div
                                    style='
                                      width: 100%;
                                      height: 22px;
                                      background-color: #003087;
                                      display: block;
                                      margin-top: 15px;
                                    '
                                  >
                                    <p style='text-align: center'>
                                      <a
                                        style='text-decoration: none; color: white'
                                        href='{link}'
                                        target='_blank'
                                        >sistema apae</a
                                      >
                                    </p>
                                  </div>
                                </div>
                              </body>
                            </html>";

        return content;
    }
}
