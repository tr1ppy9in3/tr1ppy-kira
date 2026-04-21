using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Kira.EF.DependencyInjection;

public static class ServiceCollectionDbContextBuilderExtensions
{
    public static DbContextInternalOptionsBuildeDependencyInjection<TContext> AddDataContext<TContext>
    (
        this IServiceCollection serviceCollection
    )
        where TContext : DbContext
    {
        return new DbContextInternalOptionsBuildeDependencyInjection<TContext>(serviceCollection);
    }
}