namespace VotingSystem.Shared.Models;

public record VoteParticipationRequestDto
{
    public required string UserId { get; set; }

    public required int VoteId { get; set; }
}