using VotingSystem.DataAccess.Models;

namespace VotingSystem.DataAccess.Services;

public interface IUsersService
{
    Task<IReadOnlyCollection<User>> GetLatestUsersAsync(int? count = null);
    
    Task<User> GetByIdAsync(int id);
}