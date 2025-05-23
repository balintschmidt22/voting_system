﻿using Microsoft.EntityFrameworkCore;
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
    
    public async Task<IReadOnlyCollection<Vote>> GetVotesAsync()
    {
        var query = _context.Votes
            .OrderBy(m => m.End);
        
        return await query.ToListAsync();
    }
    
    public async Task AddAsync(Vote vote, string userId)
    {
        await CheckIfQuestionExistsAsync(vote);

        vote.UserId = userId;
        
        try
        {
            await _context.Votes.AddAsync(vote);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new SaveFailedException("Failed to create vote.", ex);
        }
    }

    public async Task<IReadOnlyCollection<Vote>> GetMyVotesAsync(string? id)
    {
        var query = _context.Votes
            .Where(m => string.Equals(m.UserId, id))
            .OrderBy(m => m.End);
        
        return await query.ToListAsync();
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

    public async Task<IReadOnlyCollection<Vote>> GetBySubString(string sub, bool isActive)
    {
        IOrderedQueryable<Vote> query;
        
        if (isActive)
        {
            query = _context.Votes
                .Where(m => m.Start <= DateTime.Now && m.End >= DateTime.Now && m.Question.Contains(sub))
                .OrderBy(m => m.End);
        }
        else
        {
            query = _context.Votes
                .Where(m => m.End <= DateTime.Now && m.Question.Contains(sub))
                .OrderByDescending(m => m.End);
        }
        
        
        return await query
            .ToListAsync();
    }
    
    public async Task<IReadOnlyCollection<Vote>> GetByDate(DateTime start, DateTime end, bool isActive)
    {
        IOrderedQueryable<Vote> query;
        
        if (isActive)
        {
            var votes = await GetActiveVotesAsync();
            query = _context.Votes
                .Where(m => m.Start >= start && m.End <= end && votes.Contains(m))
                .OrderBy(m => m.End);
        }
        else
        {
            var votes = await GetClosedVotesAsync();
            query = _context.Votes
                .Where(m => m.Start >= start && m.End <= end && votes.Contains(m))
                .OrderByDescending(m => m.End);
        }
        
        
        return await query
            .ToListAsync();
    }
    
    private async Task CheckIfQuestionExistsAsync(Vote vote)
    {
        if (await _context.Votes.AnyAsync(m => m.Id != vote.Id
                                                && m.Question.ToLower().Equals(vote.Question.ToLower())))
            throw new InvalidDataException("Vote with the same question already exists");
    }
}