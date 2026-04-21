using Kira.Security.Core.Abstractions;
using Kira.Security.UseCases.Abstractions;
using MediatR;

namespace Kira.Security.Authentication.UseCases.Features.Login.Commands.SendOneTimeLoginCode;

public sealed class SendOneTimeLoginCodeHandler(
    IUserRepository userRepository,
    ILoginOtpCache otpCache,
    IEmailSender emailSender
) : IRequestHandler<SendOneTimeLoginCodeCommand>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task Handle(SendOneTimeLoginCodeCommand request, CancellationToken cancellationToken)
    {
        var userExists = await userRepository.ExistsByEmailAsync(request.Email, cancellationToken);
        if (!userExists) return; 

        var code = new Random().Next(100000, 999999).ToString();
        var state = new LoginOtpState(request.Email, code, DateTime.UtcNow.Add(LoginOtpState.TimeToLive));

        await otpCache.SaveAsync(state, cancellationToken);
        await emailSender.SendEmailAsync(
            toEmail: request.Email, 
            subject: "Вход в Kira", 
            body: $"Ваш одноразовый код для входа: {code}", 
            cancellationToken: cancellationToken
        );
    }
}