using Kira.Security.Management.Core.Abstractions;
using MediatR;
using Kira.Security.Management.UseCases.Features.Account.Faults;
using Kira.UseCases.Results;
using Kira.UseCases.Results.Implementations;

namespace Kira.Security.Management.UseCases.Features.Account.Commands.UpdateProfile;

public sealed class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result<AccountFault>>
{
    private readonly IUserProfileRepository _userProfileRepository;

    public UpdateProfileCommandHandler(IUserProfileRepository userProfileRepository)
    {
        ArgumentNullException.ThrowIfNull(userProfileRepository, nameof(userProfileRepository));
        _userProfileRepository  = userProfileRepository;
    }
    
    public async Task<Result<AccountFault>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _userProfileRepository.GetAsync(request.UserId);
        if (profile is null) return AccountFault.UserDoesntExists;

        profile.Name = request.Model.Name;
        profile.Surname = request.Model.Surname;
        profile.MiddleName = request.Model.MiddleName;
        profile.ProfilePic = request.Model.ProfilePic;
        
        await _userProfileRepository.UpdateAsync(profile);
        return ResultMarker.Succeed();
    }
}