using System.Text;
using Kira.Security.Core.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Kira.Security.Bootstrap;

internal static class AuthenticationBootstrap
{
    internal static IServiceCollection AddSecurityAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenOptions = configuration.GetSection("Token").Get<TokenOptions>();
        var key = Encoding.UTF8.GetBytes(tokenOptions!.SecretKey);

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = false,
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero 
                };
            });

        return services;
    }
}