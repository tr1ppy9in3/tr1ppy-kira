

using Kira.Security.Authentication.UseCases.Features.Session.Faults;
using Kira.Security.Host.Mapping;
using Microsoft.AspNetCore.Mvc;

public abstract class SessionFaultMapper
{
    public static IActionResult Map(SessionFault fault)
    {
        int statusCode = fault.Type switch
        {
            SessionFaultEnum.InvalidRefreshToken => StatusCodes.Status401Unauthorized,
            SessionFaultEnum.SessionExpired => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status400BadRequest
        };

        return ResultMapper.CreateProblem(fault, statusCode);
    }
}