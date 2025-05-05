using Microsoft.EntityFrameworkCore;
using VotingSystem.DataAccess.Exceptions;
using VotingSystem.DataAccess.Models;

namespace VotingSystem.DataAccess.Services;

public class VoteParticipationService : IVoteParticipationService
{
    private readonly VotingSystemDbContext _context;

    public VoteParticipationService(VotingSystemDbContext context)
    {
        _context = context;
    }
    
    public async Task<VoteParticipation> GetByIdAsync(int id)
    {
        var voteParticipation = await _context.VoteParticipations
            .FirstOrDefaultAsync(m => m.Id == id);

        if (voteParticipation is null)
            throw new EntityNotFoundException(nameof(VoteParticipation));

        return voteParticipation;
    }
    
    public async Task<IReadOnlyCollection<VoteParticipation>> GetVoteParticipationsByVoteAsync(int id)
    {
        var query = _context.VoteParticipations
            .Where(m => m.VoteId == id)
            .OrderByDescending(m => m.Id);
        
        return await query
            .ToListAsync();
    }
    
    public async Task<IReadOnlyCollection<VoteParticipation>> GetVoteParticipationsByUserAsync(string id)
    {
        var query = _context.VoteParticipations
            .Where(m => m.UserId == id)
            .OrderByDescending(m => string.Equals(m.UserId, id));
        
        return await query
            .ToListAsync();
    }
    
    public async Task AddVoteParticipationAsync(string userId, int voteId)
    {
        await CheckIfVoteExistsAsync(userId, voteId);
        await CheckIfVoteIsClosed(voteId);
        
        var vp = new VoteParticipation
        {
            UserId = userId,
            VoteId = voteId,
            VotedAt = DateTime.Now
        };
        
        try
        {
            await _context.VoteParticipations.AddAsync(vp);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new SaveFailedException("Failed to submit new vote.", ex);
        }
    }
    
    private async Task CheckIfVoteExistsAsync(string userId, int voteId)
    {
        if (await _context.VoteParticipations.AnyAsync(v => string.Equals(v.UserId, userId)
                                                       && v.VoteId == voteId))
            throw new InvalidDataException("Already voted!");
    }
    
    private async Task CheckIfVoteIsClosed(int voteId)
    {
        if (await _context.Votes.AnyAsync(v => v.Id == voteId && v.End <= DateTime.Now))
            throw new InvalidDataException("Vote is closed!");
    }
}