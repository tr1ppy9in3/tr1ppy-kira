using MediatR;

namespace Kira.UseCases.Mediator;

/// <summary>
/// Расширения для <see cref="IMediator"/>.
/// </summary>
public static class MediatorExtensions
{
    /// <summary>
    /// Асинхронно получить список объектов.
    /// </summary>
    /// <typeparam name="TObject"> Тип запрашиваемых объектов. </typeparam>
    /// <param name="mediator"> Медиатор. </param>
    /// <param name="request"> Запрос. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Список объектов. </returns>
    public static async Task<List<TObject>> GetListAsync<TObject>(this IMediator mediator,
                                                                  IStreamRequest<TObject> request,
                                                                  CancellationToken cancellationToken)
    {
        var list = new List<TObject>();

        var stream = mediator.CreateStream(request, cancellationToken);

        await foreach (var obj in stream.WithCancellation(cancellationToken))
        {
            list.Add(obj);
        }

        return list;
    }

    /// <summary>
    /// Асинхронно получить множество объектов.
    /// </summary>
    /// <typeparam name="TObject"> Тип запрашиваемых объектов. </typeparam>
    /// <param name="mediator"> Медиатор. </param>
    /// <param name="request"> Запрос. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Множество объектов. </returns>
    public static async Task<HashSet<TObject>> GetHashSetAsync<TObject>(this IMediator mediator,
                                                                        IStreamRequest<TObject> request,
                                                                        CancellationToken cancellationToken)
    {
        var set = new HashSet<TObject>();

        var stream = mediator.CreateStream(request, cancellationToken);

        await foreach (var obj in stream.WithCancellation(cancellationToken))
        {
            set.Add(obj);
        }

        return set;
    }
}
