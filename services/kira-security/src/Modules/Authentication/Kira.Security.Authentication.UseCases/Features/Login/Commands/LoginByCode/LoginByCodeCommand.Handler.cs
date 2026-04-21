using Kira.Security.Authentication.Core;
using Kira.Security.Authentication.UseCases.Features.Login.Commands.Abstractions;
using Kira.Security.Authentication.UseCases.Features.Login.Faults;
using Kira.Security.Core.Abstractions;
using Kira.Security.Core.Entities;
using Kira.Security.Core.Options;

using Kira.Security.UseCases.Abstractions;
using Kira.Security.UseCases.Services;

using Kira.UseCases.Results.Implementations;

using MediatR;
using Microsoft.Extensions.Options;

namespace Kira.Security.Authentication.UseCases.Features.Login.Commands.LoginByCode;

public sealed class LoginByOneTimeCodeCommandHandler(
    IMediator mediator,
    IClientInfoProvider clientInfoProvider,
    IUserRepository userRepository,
    ITokenService tokenService,
    TokenGenerator tokenGenerator,
    IOptions<TokenOptions> tokenOptions,
    IOptions<PasswordOptions> passwordOptions,
    ILoginOtpCache otpCache
) : BaseLoginHandler<LoginByCodeCommand>(
    mediator, clientInfoProvider, userRepository, 
    tokenService, tokenGenerator, passwordOptions, tokenOptions
)
{
    private readonly ILoginOtpCache _otpCache = otpCache;
    protected override LoginType LoginType => LoginType.ByCode;
    
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override async Task<Result<LoginFault, User>> ResolveUser(
        LoginByCodeCommand request, 
        CancellationToken cancellationToken
    )
    {
        var otpState = await _otpCache.GetAsync(request.Email, cancellationToken);
        if (otpState == null || otpState.Code != request.Code || DateTime.UtcNow > otpState.ExpiredAt)
            return LoginFault.BadCredentials;

        var user = await UserRepository.FindByEmailAsync(request.Email, cancellationToken);
        if (user == null) return LoginFault.BadCredentials;

        await _otpCache.RemoveAsync(request.Email, cancellationToken);
        return user;
    }
}