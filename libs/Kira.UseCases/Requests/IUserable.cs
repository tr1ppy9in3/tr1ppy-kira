namespace Kira.UseCases.Requests;

/// <summary>
/// Пользовательская сущность.
/// </summary>
public interface IUserable
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; }
}
