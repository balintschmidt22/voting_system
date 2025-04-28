namespace VotingSystem.Shared.Models;

public record VoteResponseDto
{
    public required int Id { get; init; }
    
    public required UserResponseDto User { get; init; }
    
    public required string Question { get; init; }
    
    public required string[] Options { get; init; }

    public required DateTime Start { get; init; }
    
    public required DateTime End { get; init; }
    
    public required ICollection<AnonymousVoteResponseDto> AnonymousVotes { get; set; }
    
    public required ICollection<VoteParticipationResponseDto> VoteParticipations { get; set; }
    
    public DateTime CreatedAt { get; init; }
    
    public DateTime UpdatedAt { get; init; }
}