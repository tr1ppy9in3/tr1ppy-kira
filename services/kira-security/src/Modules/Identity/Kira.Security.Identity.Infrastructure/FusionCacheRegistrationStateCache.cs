using Kira.Security.Identity.UseCases.Features.Registration;
using ZiggyCreatures.Caching.Fusion;

namespace Kira.Security.Identity.Infrastructure;

/// <summary>
/// Реализация кэша состояний регистрации через FusionCache (L1 In-Memory + L2 Redis).
/// </summary>
public sealed class FusionCacheRegistrationStateCache : IRegistrationStateCache
{
    private readonly IFusionCache _cache;
    private const string CacheKeyPrefix = "reg_state:";

    public FusionCacheRegistrationStateCache(IFusionCache cache)
    {
        _cache = cache;
    }

    /// <summary>
    /// Генерирует уникальный ключ для кэша на основе email.
    /// </summary>
    private static string GetCacheKey(string email) => $"{CacheKeyPrefix}{email.ToLowerInvariant()}";

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task SaveStateAsync(RegistrationState state, CancellationToken cancellationToken = default)
    {
        var key = GetCacheKey(state.Email);
        var options = new FusionCacheEntryOptions
        {
            Duration = RegistrationState.TimeToLive
        };

        await _cache.SetAsync(key, state, options, token: cancellationToken);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<RegistrationState?> GetStateAsync(string email, CancellationToken cancellationToken = default)
    {
        var key = GetCacheKey(email);
        return await _cache.GetOrDefaultAsync<RegistrationState?>(key, defaultValue: null, token: cancellationToken);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task RemoveStateAsync(string email, CancellationToken cancellationToken = default)
    {
        var key = GetCacheKey(email);
        await _cache.RemoveAsync(key, token: cancellationToken);
    }
}