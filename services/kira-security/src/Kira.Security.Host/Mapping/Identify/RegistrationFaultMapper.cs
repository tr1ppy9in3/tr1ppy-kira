using Kira.Security.Identity.UseCases.Features.Registration.Faults;
using Microsoft.AspNetCore.Mvc;

namespace Kira.Security.Host.Mapping.Identify;

public static class RegistrationFaultMapper
{
    public static IActionResult Map(RegistrationFault fault)
    {
        var statusCode = fault.Type switch
        {
            RegistrationFaultEnum.EmailAlreadyTaken => StatusCodes.Status409Conflict,
            RegistrationFaultEnum.RegistrationTimeExpired => StatusCodes.Status410Gone,
            RegistrationFaultEnum.InvalidCode => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status400BadRequest
        };

        return ResultMapper.CreateProblem(fault, statusCode);
    }
}