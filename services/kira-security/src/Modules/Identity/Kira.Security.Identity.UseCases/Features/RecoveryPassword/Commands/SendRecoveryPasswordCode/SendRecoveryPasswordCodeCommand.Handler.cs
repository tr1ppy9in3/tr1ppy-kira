using Kira.Security.Core.Abstractions;
using Kira.Security.Identity.UseCases.Features.RecoveryPassword.Faults;
using Kira.Security.UseCases.Abstractions;
using Kira.UseCases.Results;
using Kira.UseCases.Results.Implementations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Kira.Security.Identity.UseCases.Features.RecoveryPassword.Commands.SendRecoveryPasswordCode;

public class SendRecoveryPasswordCodeCommandHandler 
    : IRequestHandler<SendRecoveryPasswordCodeCommand, Result<RecoveryPasswordFault>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordRecoveryStateCache _recoveryStateCache;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<SendRecoveryPasswordCodeCommandHandler> _logger;
    
    public SendRecoveryPasswordCodeCommandHandler(
        IUserRepository userRepository,
        IPasswordRecoveryStateCache recoveryStateCache, 
        IEmailSender  emailSender,
        ILogger<SendRecoveryPasswordCodeCommandHandler> logger
    )
    {
        ArgumentNullException.ThrowIfNull(userRepository, nameof(userRepository));
        ArgumentNullException.ThrowIfNull(recoveryStateCache, nameof(recoveryStateCache));
        ArgumentNullException.ThrowIfNull(emailSender, nameof(emailSender));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        
        _userRepository = userRepository;
        _recoveryStateCache = recoveryStateCache;
        _emailSender = emailSender;
        _logger = logger;
    }
    
    public async Task<Result<RecoveryPasswordFault>> Handle(SendRecoveryPasswordCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByEmailAsync(request.Email, cancellationToken);
        if (user == null) return ResultMarker.Succeed();
        
        var code = new Random().Next(100000, 999999).ToString();
        var state = new PasswordRecoveryState(request.Email, code, DateTime.UtcNow.AddMinutes(15));
        
        await _recoveryStateCache.SaveAsync(state, cancellationToken);

        try
        {
            await _emailSender.SendEmailAsync(
                toEmail: request.Email, 
                subject: "Восстановление пароля", 
                body: $"Код: {code}",
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to send email to {email}!", request.Email);
            return RecoveryPasswordFault.FailedToSendEmail;
        }
        
        return ResultMarker.Succeed();
    }
}