using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kira.EF.DependencyInjection;

public sealed class DbContextInternalOptionsBuildeDependencyInjection<TContext>(IServiceCollection serviceCollection)
    : DbContextInternalOptionsBuilder<TContext, DbContextInternalOptionsBuildeDependencyInjection<TContext>>
    where TContext : DbContext
{
    public IServiceCollection Register()
    {
        return serviceCollection.AddScoped(serviceProvider =>
        {
            DbContextOptions<TContext>? dbContextOptions = default;
            if (IsNeedConfiguration)
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                dbContextOptions = Build(configuration);
            }
            else
            {
                dbContextOptions = Build(null);
            }

            return (TContext)Activator.CreateInstance(typeof(TContext), dbContextOptions)!;
        });
    }
}