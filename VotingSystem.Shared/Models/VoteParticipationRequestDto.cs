namespace VotingSystem.Shared.Models;

public record VoteParticipationRequestDto
{
    public required UserResponseDto User { get; set; }

    public required VoteResponseDto Vote { get; set; }
}