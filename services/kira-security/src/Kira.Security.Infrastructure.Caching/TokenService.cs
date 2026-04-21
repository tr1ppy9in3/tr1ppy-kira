using System.IdentityModel.Tokens.Jwt;
using Kira.Security.Core.Entities;
using Kira.Security.UseCases.Abstractions;
using Microsoft.AspNetCore.Http;
using StackExchange.Redis;
using ZiggyCreatures.Caching.Fusion;

namespace Kira.Security.Infrastructure.Caching;

public sealed class TokenService : ITokenService
{
    private readonly IDatabase _redisDb;
    private readonly IFusionCache _cache;
    
    private const string TokenPrefix = "refresh_token:";
    private const string UserSessionsPrefix = "user_sessions:";
    private const string BlacklistPrefix = "blacklist:";
    
    public TokenService(
        IFusionCache cache, 
        IConnectionMultiplexer redis
    )
    {
        ArgumentNullException.ThrowIfNull(cache, nameof(cache));
        ArgumentNullException.ThrowIfNull(redis, nameof(redis));
        
        _cache = cache;
        _redisDb = redis.GetDatabase();
    }
    
    private static string GetTokenKey(string token) => $"{TokenPrefix}{token}";
    private static string GetUserSessionsKey(Guid userId) => $"{UserSessionsPrefix}{userId}";
    private static string GetBlacklistKey(string token) => $"{BlacklistPrefix}{token}";



    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task SaveRefreshTokenAsync(RefreshToken token, CancellationToken cancellationToken)
    {
        var userKey = GetUserSessionsKey(token.UserId);
        var duration = token.ExpiresAt - DateTime.UtcNow;
        if (duration <= TimeSpan.Zero) return;

        await _cache.SetAsync(
            key: GetTokenKey(token.Token),
            value: token,
            options => options.SetDuration(duration),
            token: cancellationToken
        );
        
        await _redisDb.SetAddAsync(userKey, token.Token);
        await _redisDb.KeyExpireAsync(userKey, duration);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken cancellationToken)
    {
        return _cache.GetOrDefaultAsync<RefreshToken?>(
            GetTokenKey(token), 
            token: cancellationToken
        ).AsTask();
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task RemoveRefreshTokenAsync(string token, CancellationToken cancellationToken)
    {
        var tokenKey = GetTokenKey(token);
        
        var savedToken = await _cache.GetOrDefaultAsync<RefreshToken?>(
            key: tokenKey,
            token: cancellationToken
        );

        if (savedToken is not null)
        {
            await _cache.RemoveAsync(key: tokenKey, token: cancellationToken);
            await _redisDb.SetRemoveAsync(GetUserSessionsKey(savedToken.UserId), token);
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task RemoveAllUserTokensAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userKey = GetUserSessionsKey(userId);
        var tokens = await _redisDb.SetMembersAsync(userKey);
        if (tokens.Length == 0) return;

        foreach (var token in tokens)
        {
            await _cache.RemoveAsync(GetTokenKey(token!), token: cancellationToken);
        }
        
        await _redisDb.KeyDeleteAsync(userKey);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public Task DeactivateAccessTokenAsync(string token, CancellationToken cancellationToken)
    {
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(token)) return Task.CompletedTask;
        
        var jwtToken = handler.ReadJwtToken(token);
        var expiresAt = jwtToken.ValidTo;
        
        var ttl = expiresAt - DateTime.UtcNow;
        if (ttl <= TimeSpan.Zero) return Task.CompletedTask;
        
        return _cache.SetAsync(
            $"{BlacklistPrefix}{token}", 
            true, 
            options => options.SetDuration(ttl), 
            token: cancellationToken
        ).AsTask();
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public Task<bool> IsAccessTokenDeactivatedAsync(string token, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(token)) return Task.FromResult(false);
        
        return _cache.GetOrDefaultAsync<bool>(
            $"{BlacklistPrefix}{token}", 
            defaultValue: false, 
            token: cancellationToken
        ).AsTask();
    }
}