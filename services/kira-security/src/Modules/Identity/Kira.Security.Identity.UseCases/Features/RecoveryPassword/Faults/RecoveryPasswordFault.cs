using Kira.UseCases.Faults;

namespace Kira.Security.Identity.UseCases.Features.RecoveryPassword.Faults;

public record RecoveryPasswordFault : CommandFault<RecoveryPasswordFaultEnum, RecoveryPasswordFault>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public RecoveryPasswordFault() { }
    
    protected override string GetModuleNamespace() => "Login";
    
    public static RecoveryPasswordFault InvalidOrExpiredCode => 
        Create(RecoveryPasswordFaultEnum.RecoveryCodeInvalidOrExpired, "Код неверный или просрочен!");

    public static RecoveryPasswordFault  FailedToSendEmail =>
        Create(RecoveryPasswordFaultEnum.UnableToSendEmail , "Невозможно отправить код на указанную почту.");
}