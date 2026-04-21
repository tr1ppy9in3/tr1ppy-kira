using Kira.Security.Identity.Infrastructure;
using Kira.Security.Identity.UseCases.Features.RecoveryPassword;
using Kira.Security.Identity.UseCases.Features.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Kira.Security.Identity.Bootstrap;

public static class ServicesCollectionExtensions
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services)
    {
        services.AddScoped<IRegistrationStateCache, FusionCacheRegistrationStateCache>();
        services.AddScoped<IPasswordRecoveryStateCache, FusionStateCachePasswordRecoveryStateStateCache>();
        return services;
    }
}