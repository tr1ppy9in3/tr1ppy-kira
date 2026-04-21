using System.Reflection;
using Kira.Security.Management.Core;
using Kira.Security.Management.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kira.Security.Management.Infrastructure.Persistence;

public sealed class Context : DbContext
{
    internal DbSet<UserProfile> UserProfiles { get; init; }
    
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public Context(DbContextOptions options) : base(options: options)
    {
        Database.Migrate();
    }
    
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}