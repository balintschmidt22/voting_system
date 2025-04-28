using Microsoft.EntityFrameworkCore;
using VotingSystem.DataAccess.Exceptions;
using VotingSystem.DataAccess.Models;

namespace VotingSystem.DataAccess.Services;

public class AnonymousVoteService : IAnonymousVoteService
{
    private readonly VotingSystemDbContext _context;

    public AnonymousVoteService(VotingSystemDbContext context)
    {
        _context = context;
    }
    
    public async Task<AnonymousVote> GetByIdAsync(int id)
    {
        var anonymousVote = await _context.AnonymousVotes
            .FirstOrDefaultAsync(m => m.Id == id);

        if (anonymousVote is null)
            throw new EntityNotFoundException(nameof(AnonymousVote));

        return anonymousVote;
    }
    
    public async Task<IReadOnlyCollection<AnonymousVote>> GetAnonymousVotesByVoteAsync(int id)
    {
        var query = _context.AnonymousVotes
            .Where(m => m.VoteId == id)
            .OrderByDescending(m => m.Id);
        
        return await query
            .ToListAsync();
    }
    
    public async Task<IReadOnlyCollection<AnonymousVote>> GetAnonymousVotesByOptionAsync(string option)
    {
        var query = _context.AnonymousVotes
            .Where(m => string.Equals(m.SelectedOption,option))
            .OrderByDescending(m => m.Id);
        
        return await query
            .ToListAsync();
    }
}