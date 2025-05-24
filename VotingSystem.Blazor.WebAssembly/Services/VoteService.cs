using AutoMapper;
using VotingSystem.Blazor.WebAssembly.Exception;
using VotingSystem.Blazor.WebAssembly.Infrastructure;
using VotingSystem.Blazor.WebAssembly.ViewModels;
using VotingSystem.Shared.Models;

namespace VotingSystem.Blazor.WebAssembly.Services
{
    public class VoteService : BaseService, IVoteService
    {
        private readonly IMapper _mapper;
        private readonly IHttpRequestUtility _httpRequestUtility;
        private readonly VotingSystemIndexDatabase _votingSystemIndexDatabase;

        public VoteService(IMapper mapper, IHttpRequestUtility httpRequestUtility, IToastService toastService,
            VotingSystemIndexDatabase votingSystemIndexDatabase) : base(toastService)
        {
            _mapper = mapper;
            _httpRequestUtility = httpRequestUtility;
            _votingSystemIndexDatabase = votingSystemIndexDatabase;
        }

        public async Task<List<VoteViewModel>> GetVotesAsync(bool offline)
        {
            if (offline)
            {
                return await LoadVotesFromLocalDatabaseAsync();
            }

            try
            {
                var response = await _httpRequestUtility.ExecuteGetHttpRequestAsync<List<VoteResponseDto>>("votes");
                var voteViewModels = _mapper.Map<List<VoteViewModel>>(response.Response);
                await SaveVotesToDatabase(voteViewModels);
                return voteViewModels;
            }
            catch (HttpRequestErrorException exp)
            {
                await HandleHttpError(exp.Response);
            }
            return new();
        }

        public async Task<List<VoteViewModel>> LoadVotesFromLocalDatabaseAsync()
        {
            return await _votingSystemIndexDatabase.Votes.GetAllAsync<VoteViewModel>();
        }

        private async Task SaveVotesToDatabase(List<VoteViewModel> votes)
        {
            await _votingSystemIndexDatabase.Votes.ClearStoreAsync();
            await _votingSystemIndexDatabase.Votes.BatchAddAsync(votes.ToArray());
        }

        public async Task<VoteViewModel> GetVoteByIdAsync(int voteId)
        {
            try
            {
                var response = await _httpRequestUtility.ExecuteGetHttpRequestAsync<VoteResponseDto>($"votes/{voteId}");
                return _mapper.Map<VoteViewModel>(response.Response);
            }
            catch (HttpRequestErrorException exp)
            {
                await HandleHttpError(exp.Response);
            }
            return new();
        }

        public async Task CreateVoteAsync(VoteViewModel vote)
        {
            var voteRequestDto = _mapper.Map<VoteRequestDto>(vote);
            try
            {
                await _httpRequestUtility.ExecutePostHttpRequestAsync<VoteRequestDto, VoteResponseDto>("votes", voteRequestDto);
            }
            catch (HttpRequestErrorException exp)
            {
                await HandleHttpError(exp.Response);
            }
        }

        public async Task<List<VoteViewModel>> GetMyVotesAsync()
        {
            try
            {
                var response = await _httpRequestUtility.ExecuteGetHttpRequestAsync<List<VoteResponseDto>>(
                    "votes/my");
                
                return _mapper.Map<List<VoteViewModel>>(response.Response);
            }
            catch (HttpRequestErrorException exp)
            {
                await HandleHttpError(exp.Response);
                return new List<VoteViewModel>();
            }
        }


        public async Task<List<UserViewModel>> GetAllUsersAsync()
        {
            try
            {
                var response = await _httpRequestUtility.ExecuteGetHttpRequestAsync<List<UserResponseDto>>($"users");
                return _mapper.Map<List<UserViewModel>>(response.Response);
            }
            catch (HttpRequestErrorException exp)
            {
                await HandleHttpError(exp.Response);
            }
            return new();
        }
    }
}
