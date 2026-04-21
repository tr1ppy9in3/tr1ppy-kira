using Kira.Security.Management.Core;
using Kira.Security.Management.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kira.Security.Management.Infrastructure.Persistence.Entities;

public sealed class UserProfileCfg : IEntityTypeConfiguration<UserProfile>
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(x => x.UserId);
        builder.ToTable("user_profiles", Constants.SchemaName);
    }
}