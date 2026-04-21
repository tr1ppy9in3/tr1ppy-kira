using Kira.Security.Core.Entities;

namespace Kira.Security.Core.Abstractions;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task UpdateAsync(User user, CancellationToken cancellationToken);
    Task RemoveAsync(User user, CancellationToken cancellationToken);
    
    Task<User?> GetAsync(Guid userId, CancellationToken cancellationToken);
    Task<User?> FindByLoginAsync(string login, CancellationToken cancellationToken);
    Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken);
}