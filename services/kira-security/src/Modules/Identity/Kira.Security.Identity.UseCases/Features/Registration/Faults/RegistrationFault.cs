using Kira.UseCases.Faults;

namespace Kira.Security.Identity.UseCases.Features.Registration.Faults;

public sealed record RegistrationFault : CommandFault<RegistrationFaultEnum, RegistrationFault>
{
    public RegistrationFault() { }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override string GetModuleNamespace() => "Registration";

    public static RegistrationFault EmailTaken => 
        Create(RegistrationFaultEnum.EmailAlreadyTaken, "Email занят");

    public static RegistrationFault InvalidRegistrationCode =>
        Create(RegistrationFaultEnum.InvalidCode, "Неправильный код регистрации!");
    
    public static RegistrationFault RegistrationTimeExpired => 
        Create(RegistrationFaultEnum.RegistrationTimeExpired, "Registration Time Expired");
}