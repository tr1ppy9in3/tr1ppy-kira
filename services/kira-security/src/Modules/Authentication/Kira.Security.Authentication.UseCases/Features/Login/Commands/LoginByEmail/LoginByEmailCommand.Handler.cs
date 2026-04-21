using Kira.Security.Authentication.Core;
using Kira.Security.Authentication.UseCases.Features.Login.Commands.Abstractions;
using Kira.Security.Authentication.UseCases.Features.Login.Faults;
using Kira.Security.Core.Abstractions;
using Kira.Security.Core.Entities;
using Kira.Security.Core.Options;
using Kira.Security.Core.Services;

using Kira.Security.UseCases.Abstractions;
using Kira.Security.UseCases.Services;

using Kira.UseCases.Results.Implementations;

using MediatR;
using Microsoft.Extensions.Options;

namespace Kira.Security.Authentication.UseCases.Features.Login.Commands.LoginByEmail;

public sealed class LoginByEmailCommandHandler(
    IMediator mediator,
    IClientInfoProvider clientInfoProvider,
    IUserRepository userRepository,
    ITokenService tokenService,
    TokenGenerator tokenGenerator,
    IOptions<TokenOptions> tokenOptions,
    IOptions<PasswordOptions> passwordOptions
) : BaseLoginHandler<LoginByEmailCommand>(
    mediator, clientInfoProvider, userRepository, 
    tokenService, tokenGenerator, passwordOptions, tokenOptions
)
{
    protected override LoginType LoginType => LoginType.ByEmail;
    
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override async Task<Result<LoginFault, User>> ResolveUser(
        LoginByEmailCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await UserRepository.FindByEmailAsync(request.Email, cancellationToken);
        if (user == null) return LoginFault.BadCredentials;

        var hash = CryptographyService.HashPassword(request.Password, PasswordOptions.Salt);
        if (user.PasswordHash != hash) return LoginFault.BadCredentials;
        
        return user;
    }
}