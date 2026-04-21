using Kira.Security.Core.Abstractions;
using Kira.Security.Core.Options;
using Kira.Security.Core.Services;
using Kira.Security.Identity.UseCases.Features.Registration.Faults;
using Kira.Security.UseCases.Abstractions;
using Kira.UseCases.Results;
using Kira.UseCases.Results.Implementations;
using MediatR;
using Microsoft.Extensions.Options;

namespace Kira.Security.Identity.UseCases.Features.Registration.Commands.CreateRegistrationState;

public sealed class CreateRegistrationStateCommandHandler  
    : IRequestHandler<CreateRegistrationStateCommand, Result<RegistrationFault>>
{
    private readonly IUserRepository  _userRepository;
    private readonly IRegistrationStateCache  _stateCache;
    private readonly IEmailSender _emailSender;
    private readonly PasswordOptions  _passwordOptions;
    
    public CreateRegistrationStateCommandHandler(
        IUserRepository  userRepository,
        IRegistrationStateCache stateCache,
        IEmailSender emailSender,
        IOptions<PasswordOptions> passwordOptions)
    {
        _userRepository = userRepository;
        _stateCache = stateCache;
        _emailSender = emailSender;
        _passwordOptions = passwordOptions.Value;
    }
    
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<Result<RegistrationFault>> Handle(
        CreateRegistrationStateCommand request, 
        CancellationToken cancellationToken
    )
    {
        var isUserExistsByEmail = await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken);
        if (isUserExistsByEmail) 
            return RegistrationFault.EmailTaken;
        
        var verificationCode = new Random().Next(100000, 999999).ToString();
        var passwordHash = CryptographyService.HashPassword(request.Password, _passwordOptions.Salt);
        
        // Кэширование состояния
        await _stateCache.SaveStateAsync(
            state: new RegistrationState(request.Login, request.Email, passwordHash, verificationCode), 
            cancellationToken: cancellationToken
        );
        
        // Отправка кода верификации
        await _emailSender.SendEmailAsync(
            toEmail: request.Email, 
            subject: EmailTemplates.Subject, 
            body: EmailTemplates.Body(verificationCode), 
            cancellationToken: cancellationToken
        );

        return ResultMarker.Succeed();
    }

    /// <summary>
    /// Шаблоны для отправки почты.
    /// </summary>
    private static class EmailTemplates
    {
        public const string Subject = "Код подтверждения регистрации Kira";
    
        public static string Body(string verificationCode) => 
            $"""
             <div style="font-family: Arial, sans-serif; color: #333; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 8px;">
                 <h2 style="color: #2b6cb0;">Добро пожаловать в платформу Kira!</h2>
                 <p>Ваш код для подтверждения регистрации:</p>
                 <div style="background-color: #f7fafc; padding: 15px; text-align: center; border-radius: 4px; margin: 20px 0;">
                     <h1 style="color: #2b6cb0; letter-spacing: 5px; margin: 0;">{verificationCode}</h1>
                 </div>
                 <p>Код действителен в течение 1 часа.</p>
                 <hr style="border: none; border-top: 1px solid #eee; margin: 20px 0;" />
                 <p style="font-size: 12px; color: #718096;">
                     Если вы не запрашивали регистрацию, просто проигнорируйте это письмо.
                 </p>
             </div>
             """;
    }
}