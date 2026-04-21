using Kira.Security.Authentication.UseCases.Features.Login.Faults;
using Microsoft.AspNetCore.Mvc;

namespace Kira.Security.Host.Mapping.Authentication;

/// <summary>
/// Преобразование из <see cref="LoginFault"/> в <see cref="IActionResult"/>
/// </summary>
public static class LoginFaultMapper
{
    public static IActionResult Map(LoginFault fault)
    {
        int statusCode = fault.Type switch
        {
            LoginFaultEnum.BadCredentials => StatusCodes.Status401Unauthorized,
            LoginFaultEnum.OtpCodeExpiredOrDoesntExist => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status400BadRequest
        };

        return ResultMapper.CreateProblem(fault, statusCode);
    }
}