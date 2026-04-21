using Kira.Security.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kira.Security.Infrastructure.Persistence.Entities.Users;

public sealed class UserCfg : IEntityTypeConfiguration<User>
{
    private const string TableName = "users";
    
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        builder.ToTable(TableName, Constants.SchemaName);
        
        builder
            .Property(u => u.Login)
            .IsRequired()
            .HasMaxLength(50);
        
        builder
            .Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(150);
        
        builder
            .Property(u => u.PasswordHash)
            .IsRequired();
        
        builder.HasIndex(u => u.Login).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();
    }
}