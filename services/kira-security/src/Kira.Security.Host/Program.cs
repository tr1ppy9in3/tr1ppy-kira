using System.Reflection;
using System.Text.Json.Serialization;

using Kira.Security.Bootstrap;
using Kira.Security.Core.Options;
using Kira.Security.Host.Middlewares;
using Kira.Security.Infrastructure.Options;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

namespace Kira.Security.Host;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplicationBuilderHelper.Create(args);
        var application =  CreateApplication(builder);

        var logger = application.Logger;
        try
        {
            await RunAppAsync(application);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.ToString());
        }
    }
 
    private static WebApplication CreateApplication(WebApplicationBuilder builder)
    {
        var application = builder.Build();
        return application;
    }

    private static Task RunAppAsync(WebApplication application)
    {
        var appName = application.Configuration["ServiceName"]
            ?? "Kira.Security";
        
        application.UseDeveloperExceptionPage();
        application.UseSwagger();
        application.UseSwaggerUI();
        
        application.UseRouting();
        application.UseCors();
        
        application.UseAuthentication();
        application.UseMiddleware<TokenBlacklistMiddleware>();
        application.UseAuthorization();
        
        application.MapGet(string.Empty, async ctx => await ctx.Response.WriteAsync(appName)).AllowAnonymous();
        application.MapControllers();
        
        return application.RunAsync();
    }
}

file static class WebApplicationBuilderHelper
{
    public static WebApplicationBuilder Create(string[] args) =>
        WebApplication.CreateBuilder(args)
            .ConfigureConfiguration()
            .ConfigureLogging()
            .ConfigureServices()
            .ConfigureWebFeatures();
    
    private static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
    {
        // builder.Logging.ClearProviders();
        return builder;
    }   
    
    private static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddKiraSecurityMicroservice(builder.Configuration);
        return builder;
    }
    
    private static WebApplicationBuilder ConfigureConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<PasswordOptions>(builder.Configuration.GetSection("Password"));
        builder.Services.Configure<TokenOptions>(builder.Configuration.GetSection("Token"));
        builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));
        return builder;
    } 
    
    private static WebApplicationBuilder ConfigureWebFeatures(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        
        services
            .AddControllers()
            .AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        
        services.AddEndpointsApiExplorer();
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });
        
        services.AddSwaggerGen(opts =>
        {
            var basePath = AppContext.BaseDirectory;
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            opts.IncludeXmlComments(Path.Combine(basePath, xmlFile), includeControllerXmlComments: true);
            
            opts.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Description = @"Enter access token",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });

            opts.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = JwtBearerDefaults.AuthenticationScheme,
                            Type = ReferenceType.SecurityScheme
                        },
                    },
                    Array.Empty<string>()
                }
            });
            
            opts.UseAllOfToExtendReferenceSchemas();
            opts.UseAllOfForInheritance();
            opts.UseOneOfForPolymorphism();
            opts.UseInlineDefinitionsForEnums();
            opts.SelectDiscriminatorNameUsing(_ => "$type");
        });
        
        return builder;
    }
}