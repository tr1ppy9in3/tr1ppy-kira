using Kira.Security.UseCases.Abstractions;

namespace Kira.Security.Host.Middlewares;

/// <summary>
/// 
/// </summary>
/// <param name="next"></param>
public sealed class TokenBlacklistMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ITokenService tokenService)
    {
        string? authHeader = context.Request.Headers["Authorization"];
        string? token = authHeader?.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
        
        if (!string.IsNullOrEmpty(token))
        {
            if (await tokenService.IsAccessTokenDeactivatedAsync(token, context.RequestAborted))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.Headers["X-Token-Status"] = "Revoked";
                return;
            }
        }
        
        await next(context);
    }
}