using System.ComponentModel.DataAnnotations;

namespace VotingSystem.Shared.Models;

public record VoteRequestDto
{
    public required string UserId { get; init; }
    
    [MinLength(5)]
    [MaxLength(255)]
    public required string Question { get; init; }
    
    [MinLength(2)]
    [MaxLength(100)]
    public required string[] Options { get; init; }
    
    public required DateTime Start { get; init; }
    
    public required DateTime End { get; init; }
    
    public ICollection<AnonymousVoteResponseDto>? AnonymousVotes { get; set; }
    
    public ICollection<VoteParticipationResponseDto>? VoteParticipations { get; set; }
}