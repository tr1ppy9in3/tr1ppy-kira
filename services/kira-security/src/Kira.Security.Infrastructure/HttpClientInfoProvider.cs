using Kira.Security.UseCases.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Kira.Security.Infrastructure;

public sealed class HttpClientInfoProvider : IClientInfoProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public HttpClientInfoProvider(IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor, nameof(httpContextAccessor));
        _httpContextAccessor = httpContextAccessor;
    }
    
    public string GetIpAddress() => 
        _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown";

    public string GetUserAgent() => 
        _httpContextAccessor.HttpContext?.Request.Headers["UserAgent"].ToString() ?? "unknown";
}