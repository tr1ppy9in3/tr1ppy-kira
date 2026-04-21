using Kira.Bfff.Middlewares;

namespace Kira.Bfff.Extensions;

public static class BootstrapExtensions
{
    public static IServiceCollection AddBffInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
        });

        services
            .AddReverseProxy()
            .LoadFromConfig(configuration.GetSection("ReverseProxy"));

        services.AddEndpointsApiExplorer();
        return services;
    }

    public static IApplicationBuilder UseBffPipeline(this WebApplication app)
    {
        app.UseCors("AllowAll");

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/security/swagger.json", "Kira Security API");
            c.RoutePrefix = "swagger";
        });

        app.UseMiddleware<SwaggerTransformMiddleware>();
        app.MapReverseProxy();

        var serviceName = app.Configuration["ServiceName"] ?? "Kira.BFF";
        app.MapGet("/", () => serviceName);

        return app;
    }
}