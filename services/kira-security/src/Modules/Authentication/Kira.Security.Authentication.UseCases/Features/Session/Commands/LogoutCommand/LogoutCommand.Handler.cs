using Kira.Security.UseCases.Abstractions;
using Kira.Security.Authentication.UseCases.Features.Session.Faults;

using Kira.UseCases.Results;
using Kira.UseCases.Results.Implementations;

using MediatR;
using Microsoft.AspNetCore.Http;

namespace Kira.Security.Authentication.UseCases.Features.Session.Commands.LogoutCommand;

public sealed class LogoutCommandHandler(ITokenService tokenService, ITokenAccessor tokenAccessor) 
    : IRequestHandler<LogoutCommand, Result<SessionFault>>
{
    public async Task<Result<SessionFault>> Handle(LogoutCommand request, CancellationToken ct)
    {
        await tokenService.RemoveRefreshTokenAsync(request.RefreshToken, ct);
        
        var accessToken = tokenAccessor.GetCurrentAccessToken();
        if (!string.IsNullOrEmpty(accessToken))
        {
            await tokenService.DeactivateAccessTokenAsync(accessToken, ct);
        }
        
        return ResultMarker.Succeed();
    }
}