using VotingSystem.DataAccess.Models;

namespace VotingSystem.DataAccess.Services;

public interface IAnonymousVoteService
{
    Task<AnonymousVote> GetByIdAsync(int id);
    Task<IReadOnlyCollection<AnonymousVote>> GetAnonymousVotesByVoteAsync(int id);
    Task<IReadOnlyCollection<AnonymousVote>> GetAnonymousVotesByOptionAsync(string option);

}