using VotingSystem.DataAccess.Models;

namespace VotingSystem.DataAccess.Services;

public interface IVoteParticipationService
{
    Task<VoteParticipation> GetByIdAsync(int id);
    Task<IReadOnlyCollection<VoteParticipation>> GetVoteParticipationsByVoteAsync(int id);
    Task<IReadOnlyCollection<VoteParticipation>> GetVoteParticipationsByUserAsync(string id);
    Task AddVoteParticipationAsync(VoteParticipation vp);
}