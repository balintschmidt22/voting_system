namespace VotingSystem.Shared.Models;

public record AnonymousVoteResponseDto
{
    public required int Id { get; set; }

    public required VoteResponseDto Vote { get; set; }

    public required string SelectedOption { get; set; }

    public required DateTime SubmittedAt { get; set; }
};