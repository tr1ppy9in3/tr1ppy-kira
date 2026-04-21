using Kira.UseCases.Faults;
using Kira.UseCases.Results.Implementations;
using MediatR;

namespace Kira.UseCases.CommandValidation;

/// <summary>
/// Интерфейс валидатора для команд без возвращаемого значения.
/// </summary>
public interface IValidationBehavior<in TRequest, TFault> : IPipelineBehavior<TRequest, Result<TFault>>
    where TRequest : IValidatableCommand<TFault> 
    where TFault : CommandFault, IValidatableFault<TFault> 
{ }

/// <summary>
/// Интерфейс валидатора для команд со значением.
/// </summary>
public interface IValidationBehavior<in TRequest, TFault, TValue> : IPipelineBehavior<TRequest, Result<TFault, TValue>>
    where TRequest : IValidatableCommand<TFault, TValue> 
    where TFault : CommandFault, IValidatableFault<TFault> 
{ }