using Kira.Security.Management.UseCases.Features.Account.Dtos;
using Kira.Security.Management.UseCases.Features.Account.Faults;
using Kira.UseCases.Requests;

namespace Kira.Security.Management.UseCases.Features.Account.Queries.GetProfileQuery;

public sealed record GetProfileQuery : IUserableRequest<AccountFault, UserProfileDto>
{
    public Guid UserId { get; set; }
}