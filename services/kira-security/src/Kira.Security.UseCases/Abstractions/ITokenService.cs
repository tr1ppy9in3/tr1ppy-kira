using Kira.Security.Core.Entities;

namespace Kira.Security.UseCases.Abstractions;

public interface ITokenService
{
    Task SaveRefreshTokenAsync(RefreshToken token, CancellationToken cancellationToken);
    Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken cancellationToken);
    Task RemoveRefreshTokenAsync(string token, CancellationToken cancellationToken);
    Task RemoveAllUserTokensAsync(Guid userId, CancellationToken cancellationToken);
    Task DeactivateAccessTokenAsync(string token, CancellationToken cancellationToken);
    Task<bool> IsAccessTokenDeactivatedAsync(string token, CancellationToken cancellationToken);
}