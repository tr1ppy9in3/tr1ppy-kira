using Kira.Security.Management.UseCases.Features.Account.Faults;
using Kira.UseCases.Requests;

namespace Kira.Security.Management.UseCases.Features.Account.Commands.ChangePassword;

public sealed record ChangePasswordCommand(ChangePasswordCommandDto Model)
    : IUserableValidatableCommand<AccountFault>
{
    public Guid UserId { get; set; }
}