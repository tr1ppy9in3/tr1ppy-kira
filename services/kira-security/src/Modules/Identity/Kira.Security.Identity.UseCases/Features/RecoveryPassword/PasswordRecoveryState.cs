namespace Kira.Security.Identity.UseCases.Features.RecoveryPassword;

public record PasswordRecoveryState(string Email, string Code, DateTime ExpiredAt)
{
    public static TimeSpan TimeToLive = TimeSpan.FromHours(1);
};