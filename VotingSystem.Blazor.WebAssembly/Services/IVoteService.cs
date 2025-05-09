using VotingSystem.Blazor.WebAssembly.ViewModels;

namespace VotingSystem.Blazor.WebAssembly.Services;

public interface IVoteService
{
    Task<List<VoteViewModel>> GetVotesAsync(bool offline);
    Task<VoteViewModel> GetVoteByIdAsync(int voteId);
    Task CreateVoteAsync(VoteViewModel vote);
    Task<List<VoteViewModel>> GetMyVotesAsync(string userId);
    Task<List<UserViewModel>> GetAllUsersAsync();
    Task<List<VoteViewModel>> LoadVotesFromLocalDatabaseAsync();
}