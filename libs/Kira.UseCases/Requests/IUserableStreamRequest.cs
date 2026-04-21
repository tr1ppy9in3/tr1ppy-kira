using MediatR;

namespace Kira.UseCases.Requests;

/// <summary>
/// Пользовательский запрос потока объектов.
/// </summary>
/// <typeparam name="TResponse"> Тип ответа. </typeparam>
public interface IUserableStreamRequest<TResponse> : IStreamRequest<TResponse>, IUserable { }
