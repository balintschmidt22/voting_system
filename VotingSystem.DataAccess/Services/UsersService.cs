using Microsoft.EntityFrameworkCore;
using VotingSystem.DataAccess.Exceptions;
using VotingSystem.DataAccess.Models;

namespace VotingSystem.DataAccess.Services;

internal class UsersService : IUsersService
{
    private readonly VotingSystemDbContext _context;

    public UsersService(VotingSystemDbContext context)
    {
        _context = context;
    }
    
    public async Task<IReadOnlyCollection<User>> GetLatestUsersAsync(int? count = null)
    {
        var query = _context.Users
            .Where(m => !m.DeletedAt.HasValue)
            .OrderByDescending(m => m.CreatedAt);
        
        if (count is null)
        {
            return await query.ToListAsync();
        }
        
        return await query
            .Take(count.Value)
            .ToListAsync();
    }

    public async Task<User> GetByIdAsync(int id)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(m => m.Id == id && !m.DeletedAt.HasValue);

        if (user is null)
            throw new EntityNotFoundException(nameof(User));

        return user;
    }
}