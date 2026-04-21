using Kira.Security.Authentication.UseCases.Features.Session.Faults;
using Kira.Security.Identity.UseCases.Features.RecoveryPassword.Faults;
using Microsoft.AspNetCore.Mvc;

namespace Kira.Security.Host.Mapping.Identify;

public static class RecoveryPasswordFaultMapper
{
    public static IActionResult Map(RecoveryPasswordFault fault)
    {
        int statusCode = fault.Type switch
        {
            RecoveryPasswordFaultEnum.RecoveryCodeInvalidOrExpired => StatusCodes.Status400BadRequest,
            RecoveryPasswordFaultEnum.UnableToSendEmail => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };

        return ResultMapper.CreateProblem(fault, statusCode);
    }
}