using Kira.UseCases.CommandValidation;
using Kira.UseCases.Faults;

namespace Kira.UseCases.Requests;

/// <summary>
/// Пользовательская команда с автоматической валидацией (возвращает только ResultMarker).
/// </summary>
/// <typeparam name="TFault">Тип ошибки.</typeparam>
public interface IUserableValidatableCommand<TFault> : IValidatableCommand<TFault>, IUserable
    where TFault : CommandFault, IValidatableFault<TFault>
{ }

/// <summary>
/// Пользовательская команда с автоматической валидацией и возвращаемым значением.
/// </summary>
/// <typeparam name="TFault">Тип ошибки.</typeparam>
/// <typeparam name="TValue">Тип значения.</typeparam>
public interface IUserableValidatableCommand<TFault, TValue> : IValidatableCommand<TFault, TValue>, IUserable
    where TFault : CommandFault, IValidatableFault<TFault>
{ }