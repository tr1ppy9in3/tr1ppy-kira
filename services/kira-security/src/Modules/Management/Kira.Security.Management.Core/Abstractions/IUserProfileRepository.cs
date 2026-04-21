using Kira.Security.Management.Core.Entities;

namespace Kira.Security.Management.Core.Abstractions;

public interface IUserProfileRepository
{
    Task<UserProfile?> GetAsync(Guid userId,CancellationToken cancellationToken = default);
    Task AddAsync(UserProfile userProfile, CancellationToken cancellationToken = default);
    Task UpdateAsync(UserProfile userProfile, CancellationToken cancellationToken = default);
    Task RemoveAsync(UserProfile userProfile, CancellationToken cancellationToken = default);
}