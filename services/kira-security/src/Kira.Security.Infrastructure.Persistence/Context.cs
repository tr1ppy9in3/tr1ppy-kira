using System.Reflection;
using Kira.Security.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kira.Security.Infrastructure.Persistence;

public sealed class Context : DbContext
{
    internal DbSet<User> Users { get; init; }
    
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