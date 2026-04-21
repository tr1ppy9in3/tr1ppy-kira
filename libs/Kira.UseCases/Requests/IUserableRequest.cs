using Kira.UseCases.Faults;
using Kira.UseCases.Results.Implementations;
using MediatR;

namespace Kira.UseCases.Requests;

/// <summary>
/// Пользовательский запрос, возвращающий результат со значением.
/// Используется для Queries (получение данных).
/// </summary>
public interface IUserableRequest<TFault, TValue> : IRequest<Result<TFault, TValue>>, IUserable
    where TFault : CommandFault, IValidatableFault<TFault>
{ }

/// <summary>
/// Пользовательский запрос, возвращающий только результат успеха/провала.
/// </summary>
public interface IUserableRequest<TFault> : IRequest<Result<TFault>>, IUserable
    where TFault : CommandFault, IValidatableFault<TFault>
{ }