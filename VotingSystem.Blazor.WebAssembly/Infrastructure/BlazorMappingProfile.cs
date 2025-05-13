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
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options))
                .ForMember(dest => dest.Start, opt => opt.MapFrom(src => src.Start!.Value))
                .ForMember(dest => dest.End, opt => opt.MapFrom(src => src.End!.Value))
                .ForMember(dest => dest.AnonymousVotes, opt => opt.Ignore())
                .ForMember(dest => dest.VoteParticipations, opt => opt.Ignore());

            CreateMap<UserResponseDto, UserViewModel>();
            
            CreateMap<LoginViewModel, LoginRequestDto>(MemberList.Source);
        }


            /*CreateMap<MovieNotificationDto, MovieViewModel>(MemberList.Source);

            CreateMap<SeatNotificationDto, SeatViewModel>(MemberList.Source)
                .ForMember(dest => dest.ReservationId,
                opt => opt.MapFrom(src => src.Reservation == null ? (int?)null : src.Reservation.Id))
                .ForSourceMember(src => src.Reservation, opt => opt.DoNotValidate());

            CreateMap<SeatClientStatusDto, SeatStatusViewModel>()
            .ConvertUsing<SeatStatusConverter>();*/

    }

    /*public class SeatStatusConverter : ITypeConverter<SeatClientStatusDto, SeatStatusViewModel>
    {
        public SeatStatusViewModel Convert(SeatClientStatusDto source, SeatStatusViewModel destination, ResolutionContext context)
        {
            return source switch
            {
                SeatClientStatusDto.None => SeatStatusViewModel.Free,
                SeatClientStatusDto.Reserved => SeatStatusViewModel.Reserved,
                SeatClientStatusDto.Sold => SeatStatusViewModel.Sold,
                _ => throw new ArgumentOutOfRangeException(nameof(source), $"Unknown status value: {source}")
            };
        }
    }*/
}
