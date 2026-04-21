using System.Reflection;
using FluentValidation;
using Kira.UseCases.CommandValidation;
using Kira.UseCases.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace Kira.UseCases.DI;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddKiraMediatR(
        this IServiceCollection services, 
        params Assembly[] assemblies)
    {
        services.AddValidatorsFromAssemblies(assemblies, includeInternalTypes: true);
        
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(assemblies);
            cfg.AddOpenBehavior(typeof(UserContextBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        return services;
    }
}