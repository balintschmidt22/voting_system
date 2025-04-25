using AutoMapper;
using VotingSystem.DataAccess.Models;
using VotingSystem.Shared.Models;

namespace VotingSystem.WebAPI.Infrastructure;

/// <summary>
/// Automapper mappingProfile
/// </summary>
public class MappingProfile: Profile
{
    /// <summary>
    /// Constructor - Define mapping rules here
    /// </summary>
    public MappingProfile()
    {
        CreateMap<User, UserResponseDto>(MemberList.Destination);
        
        CreateMap<Vote, VoteResponseDto>(MemberList.Destination);
    }
}
