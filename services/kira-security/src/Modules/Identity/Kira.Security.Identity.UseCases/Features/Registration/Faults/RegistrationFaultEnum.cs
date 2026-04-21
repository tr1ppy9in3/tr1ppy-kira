namespace Kira.Security.Identity.UseCases.Features.Registration.Faults;

public enum RegistrationFaultEnum
{
    EmailAlreadyTaken,
    RegistrationTimeExpired,
    InvalidCode,
}