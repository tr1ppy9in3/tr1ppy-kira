using Kira.Security.Authentication.Infrastructure;
using Kira.Security.Authentication.UseCases.Features.Login;
using Kira.Security.Authentication.UseCases.Features.Login.Commands.LoginByCode;

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Kira.Security.Authentication.Bootstrap;

public static class ServicesCollectionExtensions
{
    
    public static IServiceCollection AddAuthenticationModule(this IServiceCollection services)
    {
        services.AddScoped<ILoginOtpCache, FusionCacheLoginOtpCache>();
        return services;
    }
}