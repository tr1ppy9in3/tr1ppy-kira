using Kira.Security.UseCases.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Kira.Security.Infrastructure.Caching;

public sealed class HttpTokenAccessor(IHttpContextAccessor accessor) : ITokenAccessor
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns></returns>
    public string? GetCurrentAccessToken()
    {
        string? authHeader = accessor.HttpContext?.Request.Headers["Authorization"];
        if (string.IsNullOrEmpty(authHeader)) return null;
        
        return authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) 
            ? authHeader["Bearer ".Length..].Trim() 
            : authHeader;
    }
}