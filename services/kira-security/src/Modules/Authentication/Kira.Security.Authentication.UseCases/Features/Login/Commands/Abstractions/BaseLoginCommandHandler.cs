using Kira.Security.Authentication.Core;
using Kira.Security.Authentication.Core.Events;
using Kira.Security.Authentication.UseCases.Features.Login.Faults;
using Kira.Security.Core.Abstractions;
using Kira.Security.Core.Entities;
using Kira.Security.Core.Options;

using Kira.Security.UseCases.Abstractions;
using Kira.Security.UseCases.Services;

using Kira.UseCases.Results.Implementations;

using MediatR;
using Microsoft.Extensions.Options;

namespace Kira.Security.Authentication.UseCases.Features.Login.Commands.Abstractions;

public abstract class BaseLoginHandler<TRequest>(
    IMediator mediator,
    IClientInfoProvider clientInfoProvider,
    IUserRepository userRepository,
    ITokenService tokenService,
    TokenGenerator tokenGenerator,
    IOptions<PasswordOptions> passwordOptions,
    IOptions<TokenOptions> tokenOptions
) : IRequestHandler<TRequest, Result<LoginFault, LoginResponse>> where TRequest : ILoginCommand
{
    private readonly IMediator _mediator = mediator; 
    private readonly IClientInfoProvider _clientInfoProvider = clientInfoProvider;
    private readonly ITokenService _tokenService = tokenService;
    private readonly TokenGenerator _tokenGenerator = tokenGenerator;
    
    protected readonly IUserRepository UserRepository = userRepository;
    protected readonly PasswordOptions PasswordOptions = passwordOptions.Value;
    private readonly TokenOptions _tokenOptions = tokenOptions.Value;

    /// <summary>
    /// Метод входа, который переопредлелит обработчик.
    /// </summary>
    protected virtual LoginType  LoginType { get; } = LoginType.Unknown;
    
    public async Task<Result<LoginFault, LoginResponse>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var userResult = await ResolveUser(request, cancellationToken);
        if (userResult.Failure) return userResult.GetFaultOrThrow();
        var user = userResult.GetValueOrThrow();
        
        await _mediator.Publish(new LoginSucceededDomainEvent(
            UserId: user.Id,
            Timestamp: DateTime.UtcNow,
            Type: LoginType,
            IpAddress: _clientInfoProvider.GetIpAddress(),
            UserAgent: _clientInfoProvider.GetUserAgent()
        ), cancellationToken);
        
        return await AuthenticateUserAsync(user, cancellationToken);
    }
    
    /// <summary>
    /// Метод для получения пользователя любым методом авторизации.
    /// </summary>
    protected abstract Task<Result<LoginFault, User>> ResolveUser(TRequest request, CancellationToken cancellationToken);
    
    /// <summary>
    /// Общая логика завершения входа: генерация JWT и Refresh токенов.
    /// </summary>
    protected async Task<LoginResponse> AuthenticateUserAsync(User user, CancellationToken cancellationToken)
    {
        var accessTokenPair = _tokenGenerator.GenerateAccessToken(user);
        var refreshTokenValue = _tokenGenerator.GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            Token = refreshTokenValue,
            UserId = user.Id,
            AccessTokenId = accessTokenPair.Jti,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_tokenOptions.RefreshTokenLifetimeInMinutes)
        };

        await _tokenService.SaveRefreshTokenAsync(refreshToken, cancellationToken);
        return new LoginResponse(accessTokenPair.Token, refreshTokenValue);
    }
}