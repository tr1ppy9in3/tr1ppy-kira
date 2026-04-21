using AutoMapper;
using Kira.Security.Management.Core;
using Kira.Security.Management.Core.Abstractions;
using Kira.Security.Management.Core.Entities;
using Kira.Security.Management.UseCases.Features.Account.Dtos;
using Kira.Security.Management.UseCases.Features.Account.Faults;
using Kira.UseCases.Results;
using Kira.UseCases.Results.Implementations;
using MediatR;

namespace Kira.Security.Management.UseCases.Features.Account.Queries.GetProfileQuery;

public sealed class GetProfileQueryHandler :  IRequestHandler<GetProfileQuery, Result<AccountFault, UserProfileDto>>
{
    private readonly IMapper _mapper;
    private readonly IUserProfileRepository _userProfileRepository;

    public GetProfileQueryHandler(
        IMapper mapper,
        IUserProfileRepository userProfileRepository
    )
    {
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));
        ArgumentNullException.ThrowIfNull(userProfileRepository, nameof(userProfileRepository));
        
        _userProfileRepository = userProfileRepository;
        _mapper = mapper;
    }
    
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<Result<AccountFault, UserProfileDto>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var profile = await _userProfileRepository.GetAsync(request.UserId, cancellationToken);
        if (profile is null)
        {
            profile = new UserProfile 
            { 
                UserId = request.UserId,
            };
            await _userProfileRepository.AddAsync(profile, cancellationToken);
        }
        return  ResultMarker.Succeed(_mapper.Map<UserProfileDto>(profile));
    }
}