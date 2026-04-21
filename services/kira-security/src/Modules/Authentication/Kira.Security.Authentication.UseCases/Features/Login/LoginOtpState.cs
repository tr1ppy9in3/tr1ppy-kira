namespace Kira.Security.Authentication.UseCases.Features.Login;

/// <summary>
/// 
/// </summary>
/// <param name="Email"></param>
/// <param name="Code"></param>
/// <param name="ExpiredAt"></param>
public record LoginOtpState(
    string Email,
    string Code,
    DateTime ExpiredAt
)
{
    public static TimeSpan TimeToLive { get; } = TimeSpan.FromMinutes(15);
};