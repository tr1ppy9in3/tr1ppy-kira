namespace Kira.Security.Identity.UseCases.Features.RecoveryPassword.Faults;

public enum RecoveryPasswordFaultEnum
{
    RecoveryCodeInvalidOrExpired,
    UnableToSendEmail,
}