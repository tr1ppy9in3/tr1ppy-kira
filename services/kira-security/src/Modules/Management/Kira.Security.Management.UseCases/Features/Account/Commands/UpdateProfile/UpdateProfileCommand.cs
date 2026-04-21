using Kira.Security.Management.UseCases.Features.Account.Faults;
using Kira.UseCases.Requests;

namespace Kira.Security.Management.UseCases.Features.Account.Commands.UpdateProfile;

public record UpdateProfileCommand(UpdateProfileCommandDto Model) : IUserableValidatableCommand<AccountFault>
{
    public Guid UserId { get; set; }
}