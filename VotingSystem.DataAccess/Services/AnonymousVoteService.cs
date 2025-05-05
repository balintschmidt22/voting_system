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
    
    public async Task AddAnonymousVoteAsync(int voteId, string option)
    {
        await CheckIfOptionExistsAsync(voteId, option);
        await CheckIfVoteIsClosed(voteId);

        var av = new AnonymousVote
        {
            VoteId = voteId,
            SelectedOption = option,
            SubmittedAt = DateTime.Now
        };

        try
        {
            await _context.AnonymousVotes.AddAsync(av);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new SaveFailedException("Failed to submit new vote.", ex);
        }
    }
    
    private async Task CheckIfOptionExistsAsync(int id, string option)
    {
        if (await _context.Votes.AnyAsync(v => v.Id == id && !v.Options.Contains(option)))
            throw new InvalidDataException("Vote does not have this option!");
    }
    
    private async Task CheckIfVoteIsClosed(int voteId)
    {
        if (await _context.Votes.AnyAsync(v => v.Id == voteId && v.End <= DateTime.Now))
            throw new InvalidDataException("Vote is closed!");
    }
    
    public async Task<Dictionary<string, int>> GetVoteResultsAsync(int voteId)
    {
        var votes = await _context.AnonymousVotes
            .Where(v => v.VoteId == voteId)
            .ToListAsync();

        return votes
            .GroupBy(v => v.SelectedOption)
            .ToDictionary(g => g.Key, g => g.Count());
    }

}