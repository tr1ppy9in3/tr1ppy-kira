using Kira.Security.Authentication.UseCases.Features.Login;
using ZiggyCreatures.Caching.Fusion;

namespace Kira.Security.Authentication.Infrastructure;

/// <summary>
/// Реализация кэша OTP-кодов через FusionCache (L1 + L2).
/// </summary>
public sealed class FusionCacheLoginOtpCache : ILoginOtpCache
{
    private readonly IFusionCache _cache;
    private const string KeyPrefix = "login_otp:";
    
    public FusionCacheLoginOtpCache(IFusionCache cache)
    {
        ArgumentNullException.ThrowIfNull(cache);
        _cache = cache;
    }
    
    private static string GetKey(string email) => $"{KeyPrefix}{email.ToLowerInvariant()}";
    
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task SaveAsync(LoginOtpState state, CancellationToken cancellationToken)
    {
        var key = GetKey(state.Email);
        await _cache.SetAsync(
            key, 
            state, 
            options => options.SetDuration(LoginOtpState.TimeToLive), 
            token: cancellationToken
        );
    }
    
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<LoginOtpState?> GetAsync(string email, CancellationToken cancellationToken)
    {
        var key = GetKey(email);

        return await _cache.GetOrDefaultAsync<LoginOtpState?>(
            key, 
            defaultValue: null, 
            token: cancellationToken
        );
    }
    
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task RemoveAsync(string email, CancellationToken cancellationToken)
    {
        var key = GetKey(email);
        await _cache.RemoveAsync(key, token: cancellationToken);
    }
}