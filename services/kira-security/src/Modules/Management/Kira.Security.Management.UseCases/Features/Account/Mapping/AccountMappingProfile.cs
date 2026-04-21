using AutoMapper;
using Kira.Security.Management.Core;
using Kira.Security.Management.Core.Entities;
using Kira.Security.Management.UseCases.Features.Account.Dtos;

namespace Kira.Security.Management.UseCases.Features.Account.Mapping;

public class AccountMappingProfile : Profile
{
    public AccountMappingProfile()
    {
        CreateMap<UserProfile, UserProfileDto>();
    }
}