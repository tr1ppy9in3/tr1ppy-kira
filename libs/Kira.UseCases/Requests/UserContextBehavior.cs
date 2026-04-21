using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Kira.UseCases.Requests;

public sealed class UserContextBehavior<TRequest, TResponse>(
    IHttpContextAccessor httpContextAccessor) 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IUserable 
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        var userIdClaim = httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(userIdClaim, out var userId)) return await next(cancellationToken);
        
        var prop = request.GetType().GetProperty(nameof(IUserable.UserId));
        prop?.SetValue(request, userId);

        return await next(cancellationToken);
    }
}