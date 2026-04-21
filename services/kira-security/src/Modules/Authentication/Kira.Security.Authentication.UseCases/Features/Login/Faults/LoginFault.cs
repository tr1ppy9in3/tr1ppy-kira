using Kira.UseCases.Faults;

namespace Kira.Security.Authentication.UseCases.Features.Login.Faults;

public record LoginFault : CommandFault<LoginFaultEnum, LoginFault>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public LoginFault() { }
    
    protected override string GetModuleNamespace() => "Login";
    
    public static LoginFault BadCredentials => 
        Create(LoginFaultEnum.BadCredentials, "Неверные учетные данные!");

    public static LoginFault OtpCodeExpiredOrDoesntExist =>
        Create(LoginFaultEnum.OtpCodeExpiredOrDoesntExist, "Одноразовый код доступа истек или недействителен!");
}