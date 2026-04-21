namespace Kira.Security.Identity.UseCases.Features.Registration;

public interface IRegistrationStateCache
{
    Task SaveStateAsync(RegistrationState state, CancellationToken cancellationToken = default);
    Task<RegistrationState?> GetStateAsync(string email, CancellationToken cancellationToken = default);
    Task RemoveStateAsync(string email, CancellationToken cancellationToken = default);
}