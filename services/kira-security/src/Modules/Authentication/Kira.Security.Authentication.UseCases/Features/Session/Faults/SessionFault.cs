using Kira.UseCases.Faults;

namespace Kira.Security.Authentication.UseCases.Features.Session.Faults;

public record SessionFault : CommandFault<SessionFaultEnum, SessionFault>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public SessionFault() { }
    
    protected override string GetModuleNamespace() => "Session";
    
    public static SessionFault SessionExpired => 
        Create(SessionFaultEnum.SessionExpired, "Сессия истекла!");

    public static SessionFault InvalidToken  =>
        Create(SessionFaultEnum.InvalidRefreshToken, "Неверный токен обновления!");
}