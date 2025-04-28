namespace VotingSystem.Shared.Models;

public record VoteParticipationResponseDto
{
    public required int Id { get; set; }
    
    public required UserResponseDto User { get; set; }

    public required VoteResponseDto Vote { get; set; }

    public required DateTime VotedAt { get; set; }
};