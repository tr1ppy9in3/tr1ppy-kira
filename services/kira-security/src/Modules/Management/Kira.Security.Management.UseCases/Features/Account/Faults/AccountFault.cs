using Kira.UseCases.Faults;

namespace Kira.Security.Management.UseCases.Features.Account.Faults;

public sealed record AccountFault : CommandFault<AccountFaultEnum, AccountFault>
{
    protected override string GetModuleNamespace() => "Account";

    public static AccountFault UserDoesntExists => Create(
        type: AccountFaultEnum.UserDoesNotExist,
        message: "Пользователь не существует!"
    );
    
    public static AccountFault OldPasswordDoesntMatch => Create(
        AccountFaultEnum.OldPasswordDoesNotMatch, 
        "Неправильный старый пароль!"
    );
}