using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Kira.Security.Authentication.UseCases.Features.Login.Commands;
using Kira.Security.Authentication.UseCases.Features.Session.Faults;
using Kira.Security.Core.Abstractions;
using Kira.Security.Core.Entities;
using Kira.Security.Core.Options;
using Kira.Security.UseCases.Abstractions;
using Kira.Security.UseCases.Services;

using Kira.UseCases.Results;
using Kira.UseCases.Results.Implementations;

using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Kira.Security.Authentication.UseCases.Features.Session.Commands.RefreshCommand;

public sealed class RefreshTokenCommandHandler(
    ITokenService tokenService,
    ITokenAccessor tokenAccessor,
    IUserRepository userRepository,
    TokenGenerator tokenGenerator,
    IOptions<TokenOptions> tokenOptions) 
    : IRequestHandler<RefreshTokenCommand, Result<SessionFault, LoginResponse>>
{
    private readonly TokenOptions _options = tokenOptions.Value;

    public async Task<Result<SessionFault, LoginResponse>> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        var oldAccessToken = tokenAccessor.GetCurrentAccessToken();
        if (oldAccessToken == null) return SessionFault.InvalidToken;
        
        var jti = GetJtiFromExpiredToken(oldAccessToken);
        if (jti == null) return SessionFault.InvalidToken;

        var savedSession = await tokenService.GetRefreshTokenAsync(request.RefreshToken, ct);
        if (savedSession == null) return SessionFault.InvalidToken;

        var bindingResult = await ValidateTokenBindingAsync(savedSession, jti, ct);
        if (bindingResult.Failure) return bindingResult.GetFaultOrThrow();

        await RevokeOldSessionAsync(request.RefreshToken, oldAccessToken, ct);

        var user = await userRepository.GetAsync(savedSession.UserId, ct);
        if (user == null) return SessionFault.InvalidToken;

        return await CreateNewSessionAsync(user, ct);
    }

    /// <summary>
    /// Извлекает идентификатор токена (JTI) без валидации времени жизни.
    /// </summary>
    private string? GetJtiFromExpiredToken(string? token)
    {
        if (string.IsNullOrEmpty(token)) return null;

        var principal = GetPrincipalFromExpiredToken(token);
        var claim = principal?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);
        return claim?.Value;
    }

    /// <summary>
    /// Проверяет соответствие JTI. В случае несовпадения - аннулирует все сессии.
    /// </summary>
    private async Task<Result<SessionFault>> ValidateTokenBindingAsync(RefreshToken session, string jti, CancellationToken ct)
    {
        if (session.AccessTokenId == jti) 
            return ResultMarker.Succeed();
        
        await tokenService.RemoveAllUserTokensAsync(session.UserId, ct);
        return SessionFault.InvalidToken;
    }

    /// <summary>
    /// Удаляет старый Refresh и заносит Access в черный список.
    /// </summary>
    private async Task RevokeOldSessionAsync(string refreshToken, string accessToken, CancellationToken ct)
    {
        await tokenService.RemoveRefreshTokenAsync(refreshToken, ct);
        await tokenService.DeactivateAccessTokenAsync(accessToken, ct);
    }

    /// <summary>
    /// Генерирует новую пару токенов и сохраняет сессию в Redis.
    /// </summary>
    private async Task<Result<SessionFault, LoginResponse>> CreateNewSessionAsync(User user, CancellationToken ct)
    {
        var (newAccessToken, newJti) = tokenGenerator.GenerateAccessToken(user);
        var newRefreshTokenValue = tokenGenerator.GenerateRefreshToken();

        var newSession = new RefreshToken
        {
            Token = newRefreshTokenValue,
            UserId = user.Id,
            AccessTokenId = newJti,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_options.RefreshTokenLifetimeInMinutes)
        };

        await tokenService.SaveRefreshTokenAsync(newSession, ct);

        return new LoginResponse(newAccessToken, newRefreshTokenValue);
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                ValidateLifetime = false,
                ValidIssuer = _options.Issuer,
                ValidAudience = _options.Audience
            };

            return tokenHandler.ValidateToken(token, parameters, out _);
        }
        catch { return null; }
    }
}