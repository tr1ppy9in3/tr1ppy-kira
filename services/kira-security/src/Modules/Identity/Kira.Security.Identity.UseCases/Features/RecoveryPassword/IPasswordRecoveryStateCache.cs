namespace Kira.Security.Identity.UseCases.Features.RecoveryPassword;

public interface IPasswordRecoveryStateCache
{
    Task SaveAsync(PasswordRecoveryState state, CancellationToken cancellationToken);
    Task<PasswordRecoveryState?> GetAsync(string email, CancellationToken cancellationToken);
    Task RemoveAsync(string email, CancellationToken cancellationToken);
}