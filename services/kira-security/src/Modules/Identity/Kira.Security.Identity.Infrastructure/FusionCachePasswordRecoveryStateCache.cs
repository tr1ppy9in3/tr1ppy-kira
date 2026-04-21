using Kira.Security.Identity.UseCases.Features.RecoveryPassword;
using ZiggyCreatures.Caching.Fusion;

namespace Kira.Security.Identity.Infrastructure;

/// <summary>
/// Реализация кэша для кодов восстановления пароля через FusionCache (L1 + L2).
/// </summary>
public class FusionStateCachePasswordRecoveryStateStateCache : IPasswordRecoveryStateCache
{
    private readonly IFusionCache _cache;
    private const string KeyPrefix = "pwd_recovery:";

    public FusionStateCachePasswordRecoveryStateStateCache(IFusionCache cache)
    {
        ArgumentNullException.ThrowIfNull(cache, nameof(cache));
        _cache = cache;
    }
    
    /// <summary>
    /// Генерирует уникальный ключ для кэша на основе email.
    /// </summary>
    private static string GetKey(string email) => $"{KeyPrefix}{email.ToLowerInvariant()}";
    
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public Task SaveAsync(PasswordRecoveryState state, CancellationToken cancellationToken)
    {
        return _cache.SetAsync(
            key: GetKey(state.Email), 
            value: state, 
            ops => ops.SetDuration(PasswordRecoveryState.TimeToLive),
            token: cancellationToken 
        ).AsTask();
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public Task<PasswordRecoveryState?> GetAsync(string email, CancellationToken cancellationToken)
    {
        return _cache.GetOrDefaultAsync<PasswordRecoveryState?>(
            key: GetKey(email),
            defaultValue: null,
            token: cancellationToken
        ).AsTask();
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public Task RemoveAsync(string email, CancellationToken cancellationToken)
    {
        return _cache.RemoveAsync(
            key: GetKey(email), 
            token: cancellationToken
        ).AsTask();
    }
}