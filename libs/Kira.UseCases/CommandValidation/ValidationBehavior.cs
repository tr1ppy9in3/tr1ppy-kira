using System.Reflection;
using FluentValidation;
using Kira.UseCases.Results.Implementations;
using MediatR;

namespace Kira.UseCases.CommandValidation;

public sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators) 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (!validators.Any()) return await next();

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .Select(f => f.ErrorMessage)
            .ToArray();

        if (failures.Length > 0)
        {
            return CreateValidationResult(failures);
        }

        return await next();
    }

    /// <summary>
    /// Магия рефлексии: динамически создает Result<TFault> или Result<TFault, TValue>
    /// </summary>
    private static TResponse CreateValidationResult(string[] failures)
    {
        var responseType = typeof(TResponse);

        // 1. Убеждаемся, что TResponse - это наш Result<> или Result<,>
        if (!responseType.IsGenericType || 
            (responseType.GetGenericTypeDefinition() != typeof(Result<>) && 
             responseType.GetGenericTypeDefinition() != typeof(Result<,>)))
        {
            throw new InvalidOperationException("ValidationBehavior работает только с типами Result");
        }

        var faultType = responseType.GetGenericArguments()[0];
        var validationMethod = faultType.GetMethod("ValidationError", 
            BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

        var faultObject = validationMethod!.Invoke(null, new object[] { failures, "Ошибка валидации" });
        var failMethod = responseType.GetMethod("Fail", BindingFlags.Public | BindingFlags.Static);
        
        return (TResponse)failMethod!.Invoke(null, new[] { faultObject })!;
    }
}