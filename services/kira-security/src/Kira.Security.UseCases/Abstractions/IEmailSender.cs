namespace Kira.Security.UseCases.Abstractions;

public interface IEmailSender
{
    Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken cancellationToken = default);
}