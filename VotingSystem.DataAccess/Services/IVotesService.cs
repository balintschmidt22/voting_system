using VotingSystem.DataAccess.Models;

namespace VotingSystem.DataAccess.Services;

public interface IVotesService
{
    Task<IReadOnlyCollection<Vote>> GetActiveVotesAsync(int? count = null);
    
    Task<IReadOnlyCollection<Vote>> GetClosedVotesAsync(int? count = null);
    
    Task<Vote> GetByIdAsync(int id);

    Task<IReadOnlyCollection<Vote>> GetBySubString(string sub, bool isActive);
}