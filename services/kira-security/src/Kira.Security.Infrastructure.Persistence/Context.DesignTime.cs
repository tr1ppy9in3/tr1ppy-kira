using Kira.EF;
using Kira.EF.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Kira.Security.Infrastructure.Persistence;

public sealed class ContextForMigration : IDesignTimeDbContextFactory<Context>
{
    public Context CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<Context>();
        var dbSettings = MigrationHelper.FindSettings();

        optionsBuilder.ApplyProvider(dbSettings.Item1, dbSettings.Item2);
        return new Context(optionsBuilder.Options);
    }
}