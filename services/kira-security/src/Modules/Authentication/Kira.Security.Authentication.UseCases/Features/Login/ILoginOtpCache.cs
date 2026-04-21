namespace Kira.Security.Authentication.UseCases.Features.Login;

public interface ILoginOtpCache
{
    Task SaveAsync(LoginOtpState state, CancellationToken cancellationToken);
    Task<LoginOtpState?> GetAsync(string email, CancellationToken cancellationToken);
    Task RemoveAsync(string email, CancellationToken cancellationToken);
}