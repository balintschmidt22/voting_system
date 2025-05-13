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
        CreateMap<UserRequestDto, User>(MemberList.Source)
            .ForSourceMember(src => src.Password, opt => opt.DoNotValidate());

        CreateMap<User, UserResponseDto>(MemberList.Destination);

        CreateMap<VoteRequestDto, Vote>(MemberList.None);
                   
        CreateMap<Vote, VoteResponseDto>(MemberList.Destination);
        
        CreateMap<VoteParticipation, VoteParticipationResponseDto>(MemberList.Destination);
            
        CreateMap<AnonymousVote, AnonymousVoteResponseDto>(MemberList.Destination);
    }
}
