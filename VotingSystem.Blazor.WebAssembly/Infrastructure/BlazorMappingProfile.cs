using AutoMapper;
using VotingSystem.Blazor.WebAssembly.ViewModels;
using VotingSystem.Shared.Models;

namespace VotingSystem.Blazor.WebAssembly.Infrastructure
{
    public class BlazorMappingProfile : Profile
    {
        public BlazorMappingProfile()
        {
            CreateMap<VoteResponseDto, VoteViewModel>()
                .ForMember(dest => dest.OptionsRaw,
                    opt => opt.MapFrom(src => string.Join(";", src.Options)))
                .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
                .ForMember(dest => dest.Start, opt => opt.MapFrom(src => src.Start))
                .ForMember(dest => dest.End, opt => opt.MapFrom(src => src.End))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id));

            CreateMap<VoteViewModel, VoteRequestDto>()
                .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options))
                .ForMember(dest => dest.Start, opt => opt.MapFrom(src => src.Start!.Value))
                .ForMember(dest => dest.End, opt => opt.MapFrom(src => src.End!.Value));

            CreateMap<UserResponseDto, UserViewModel>();
            
            CreateMap<LoginViewModel, LoginRequestDto>(MemberList.Source);
        }
    }
}
