using Kira.EF.DependencyInjection;
using Kira.Security.Authentication.Bootstrap;
using Kira.Security.Core.Abstractions;
using Kira.Security.Identity.Bootstrap;
using Kira.Security.Infrastructure;
using Kira.Security.Infrastructure.Caching;
using Kira.Security.Infrastructure.Persistence;
using Kira.Security.Infrastructure.Persistence.Entities.Users;
using Kira.Security.Management.Bootstrap;
using Kira.Security.UseCases.Abstractions;
using Kira.Security.UseCases.Services;
using Kira.UseCases.DI;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using ZiggyCreatures.Caching.Fusion;

namespace Kira.Security.Bootstrap;

public static class SecurityBootstrapExtensions
{
    public static IServiceCollection AddKiraSecurityMicroservice(this IServiceCollection services, IConfiguration configuration)
    {
        var assemblies = new[]
        {
            AuthenticationAssembly.Get,
            IdentityAssembly.Get,
            ManagementAssembly.Get,
        };

        services.AddCaching(configuration);
        services.AddPersistence(configuration);
        services.AddInfrastructure(configuration);
        
        services.AddIdentityModule();
        services.AddManagementModule();
        services.AddAuthenticationModule();
        
        services.AddKiraMediatR(assemblies);
        services.AddSecurityAuthentication(configuration);
        
        return services;
    }

    private static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("Redis") ?? throw new ArgumentNullException("RedisConnectionString");
        var redisMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
        services.AddSingleton<IConnectionMultiplexer>(redisMultiplexer);
        
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
        });
        
        services
            .AddFusionCache()
            .WithSystemTextJsonSerializer() 
            .WithDistributedCache(sp => sp.GetRequiredService<IDistributedCache>());

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDataContext<Context>()
            .WithProviderFromConfiguration("DbProvider")
            .Register();
        
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        
        services.AddScoped<IEmailSender, SmtpEmailSender>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ITokenAccessor, HttpTokenAccessor>();
        services.AddScoped<IClientInfoProvider, HttpClientInfoProvider>();
        services.AddScoped<TokenGenerator>();
        
        return services;
    }
}