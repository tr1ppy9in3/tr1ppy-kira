using Kira.UseCases.Faults;
using Kira.UseCases.Results.Implementations;
using MediatR;

namespace Kira.UseCases.CommandValidation;

/// <summary>
/// Команда с автоматической валидацией.
/// </summary>
/// <typeparam name="TFault">Тип ошибки, поддерживающий создание из валидации.</typeparam>
public interface IValidatableCommand<TFault> : IRequest<Result<TFault>>
    where TFault : CommandFault, IValidatableFault<TFault> 
{ }

/// <summary>
/// Команда со значением и автоматической валидацией.
/// </summary>
/// <typeparam name="TFault">Тип ошибки, поддерживающий создание из валидации.</typeparam>
/// <typeparam name="TValue">Тип возвращаемого значения.</typeparam>
public interface IValidatableCommand<TFault, TValue> : IRequest<Result<TFault, TValue>>
    where TFault : CommandFault, IValidatableFault<TFault>
{ }