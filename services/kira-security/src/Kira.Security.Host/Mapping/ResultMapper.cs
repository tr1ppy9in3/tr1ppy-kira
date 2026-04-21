using Kira.Security.Authentication.UseCases.Features.Login.Faults;
using Kira.Security.Authentication.UseCases.Features.Session.Faults;
using Kira.Security.Host.Mapping.Authentication;
using Kira.Security.Host.Mapping.Identify;
using Kira.Security.Identity.UseCases.Features.RecoveryPassword.Faults;
using Kira.Security.Identity.UseCases.Features.Registration.Faults;
using Kira.UseCases.Faults;
using Kira.UseCases.Results.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Kira.Security.Host.Mapping;

public static class ResultMapper
{
    public static IActionResult ToOkActionResult<TFault, TValue>(this Result<TFault, TValue> result) 
        where TFault : CommandFault
    {
        return result.Success 
            ? new OkObjectResult(result.GetValueOrThrow()) 
            : result.GetFaultOrThrow().ToActionResult();
    }
    
    public static IActionResult ToActionResult<TFault>(
        this Result<TFault> result, 
        Func<IActionResult>? onSuccess = null) 
        where TFault : CommandFault
    {
        if (result.Success)
        {
            return onSuccess != null 
                ? onSuccess() 
                : new OkResult();
        }

        return result.GetFaultOrThrow().ToActionResult();
    }

    public static IActionResult ToActionResult<TFault, TValue>(
        this Result<TFault, TValue> result, 
        Func<TValue, IActionResult>? onSuccess = null) 
        where TFault : CommandFault
    {
        if (!result.Success) 
            return result.GetFaultOrThrow().ToActionResult();
        
        var value = result.GetValueOrThrow();
        return onSuccess != null 
            ? onSuccess(value) 
            : new OkObjectResult(value);
    }

    public static IActionResult ToActionResult(this CommandFault fault)
    {
        if (fault.IsGeneral)
        {
            return fault.Code switch
            {
                "ValidationError" => CreateProblem(fault, StatusCodes.Status400BadRequest),
                "InternalError" => CreateProblem(fault, StatusCodes.Status500InternalServerError),
                _ => CreateProblem(fault, StatusCodes.Status400BadRequest)
            };
        }
        
        return fault switch
        {
            LoginFault f => LoginFaultMapper.Map(f),
            RegistrationFault f => RegistrationFaultMapper.Map(f),
            SessionFault f => SessionFaultMapper.Map(f),
            RecoveryPasswordFault f => RecoveryPasswordFaultMapper.Map(f),
            _ => CreateProblem(fault, StatusCodes.Status400BadRequest)
        };
    }
    
    public static ObjectResult CreateProblem(CommandFault fault, int statusCode)
    {
        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = fault.FullCode,
            Detail = fault.Message
        };

        if (fault.Errors is { Length: > 0 })
        {
            problem.Extensions["errors"] = fault.Errors;
        }

        return new ObjectResult(problem) { StatusCode = statusCode };
    }
}