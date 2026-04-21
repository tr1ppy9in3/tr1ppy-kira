using Kira.Security.Core.Abstractions;
using Kira.Security.Core.Entities;
using Kira.Security.UseCases.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Kira.Security.Infrastructure.Persistence.Entities.Users;

public sealed class UserRepository(Context context) : IUserRepository
{
    private readonly Context _context = context;
    
    public Task AddAsync(User user, CancellationToken cancellationToken)
    {
        _context.Users.Add(user);
        return _context.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        _context.Users.Update(user);
        return _context.SaveChangesAsync(cancellationToken);
    }

    public Task RemoveAsync(User user, CancellationToken cancellationToken)
    {
        _context.Users.Remove(user);
        return _context.SaveChangesAsync(cancellationToken);
    }

    public Task<User?> GetAsync(Guid userId, CancellationToken cancellationToken)
    {
        return _context.Users
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
    }
    
    public Task<User?> FindByLoginAsync(string login, CancellationToken cancellationToken)
    {
        return _context.Users
            .FirstOrDefaultAsync(u => u.Login == login, cancellationToken);
    }

    public Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return _context.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return _context.Users.AnyAsync(u => u.Email == email, cancellationToken);
    }
}