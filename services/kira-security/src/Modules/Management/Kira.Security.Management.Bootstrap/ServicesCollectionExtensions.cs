using Kira.EF.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

using Kira.Security.Management.Core.Abstractions;
using Kira.Security.Management.Infrastructure.Persistence;
using Kira.Security.Management.Infrastructure.Persistence.Entities;
using Kira.Security.Management.UseCases.Features.Account.Mapping;

namespace Kira.Security.Management.Bootstrap;

public static class ServicesCollectionExtensions
{
    public static IServiceCollection AddManagementModule(this IServiceCollection services)
    {
        services
            .AddDataContext<Context>()
            .WithProviderFromConfiguration("DbProvider")
            .Register();
        
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();

        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<AccountMappingProfile>();
        });
        
        return services;
    }
}