using Microsoft.EntityFrameworkCore;
using VotingSystem.DataAccess.Exceptions;
using VotingSystem.DataAccess.Models;

namespace VotingSystem.DataAccess.Services;

public class VotesService : IVotesService
{
    private readonly VotingSystemDbContext _context;

    public VotesService(VotingSystemDbContext context)
    {
        _context = context;
    }
    
    public async Task<IReadOnlyCollection<Vote>> GetActiveVotesAsync(int? count = null)
    {
        var query = _context.Votes
            .Where(m => m.Start <= DateTime.Now && m.End >= DateTime.Now)
            .OrderBy(m => m.End);
        
        if (count is null)
        {
            return await query.ToListAsync();
        }
        
        return await query
            .Take(count.Value)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<Vote>> GetClosedVotesAsync(int? count = null)
    {
        var query = _context.Votes
            .Where(m => m.End <= DateTime.Now)
            .OrderByDescending(m => m.End);
        
        if (count is null)
        {
            return await query.ToListAsync();
        }
        
        return await query
            .Take(count.Value)
            .ToListAsync();
    }

    public async Task<Vote> GetByIdAsync(int id)
    {
        var vote = await _context.Votes
            .FirstOrDefaultAsync(m => m.Id == id);

        if (vote is null)
            throw new EntityNotFoundException(nameof(Vote));

        return vote;
    }
}