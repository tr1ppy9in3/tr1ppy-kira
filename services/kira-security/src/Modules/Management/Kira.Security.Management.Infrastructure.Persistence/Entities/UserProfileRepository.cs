using Kira.Security.Management.Core;
using Kira.Security.Management.Core.Abstractions;
using Kira.Security.Management.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kira.Security.Management.Infrastructure.Persistence.Entities;

public sealed class UserProfileRepository : IUserProfileRepository
{
    private readonly Context _context;
    
    public UserProfileRepository(Context context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _context = context;
    }
    
    private DbSet<UserProfile> UserProfiles => _context.UserProfiles;
    
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public Task<UserProfile?> GetAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return UserProfiles.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public Task AddAsync(UserProfile userProfile,  CancellationToken cancellationToken = default)
    {
        UserProfiles.Add(userProfile);
        return _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public Task UpdateAsync(UserProfile userProfile, CancellationToken cancellationToken = default)
    {
        UserProfiles.Update(userProfile);
        return _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public Task RemoveAsync(UserProfile userProfile, CancellationToken cancellationToken = default)
    {
        UserProfiles.Remove(userProfile);
        return _context.SaveChangesAsync(cancellationToken);
    }
}