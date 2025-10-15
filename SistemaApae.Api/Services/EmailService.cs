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

    public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Envia email
    /// </summary>
    public async Task<bool> SendEmailAsync(string email, string name, string newPassword, EmailReasonEnum emailReason)
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

            using var mail = new MailMessage();
            mail.From = new MailAddress(senderEmail, senderName);
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
            mail.IsBodyHtml = false;

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
