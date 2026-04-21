using System.Net;
using System.Net.Mail;

using Kira.Security.Infrastructure.Options;
using Kira.Security.UseCases.Abstractions;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kira.Security.Infrastructure;

/// <summary>
/// Реализация сервиса отправки писем через SMTP.
/// </summary>
public sealed class SmtpEmailSender : IEmailSender, IDisposable
{
    private readonly SmtpOptions _options;
    private readonly ILogger<SmtpEmailSender> _logger;
    private readonly SmtpClient _smtpClient;

    public SmtpEmailSender(
        IOptions<SmtpOptions> options, 
        ILogger<SmtpEmailSender> logger)
    {
        _options = options.Value;
        _logger = logger;
        _smtpClient = new SmtpClient(_options.Host, _options.Port)
        {
            Credentials = new NetworkCredential(_options.Username, _options.Password),
            EnableSsl = _options.EnableSsl
        };
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task SendEmailAsync(
        string toEmail, 
        string subject, 
        string body, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var mailMessage = new MailMessage();
            
            mailMessage.From = new MailAddress(_options.Username, _options.SenderName);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;
            mailMessage.To.Add(toEmail);

            _logger.LogInformation("Отправка email на адрес: {Email}", toEmail);
            await _smtpClient.SendMailAsync(mailMessage, cancellationToken);
            _logger.LogInformation("Email успешно отправлен на адрес: {Email}", toEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при отправке email на адрес: {Email}. Проверьте настройки SMTP.", toEmail);
            throw; 
        }
    }

    /// <summary>
    /// Освобождаем ресурсы SmtpClient.
    /// </summary>
    public void Dispose()
    {
        _smtpClient.Dispose();
    }
}