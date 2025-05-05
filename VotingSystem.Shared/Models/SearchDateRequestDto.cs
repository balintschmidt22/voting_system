namespace VotingSystem.Shared.Models;

public record SearchDateRequestDto
{
    public required DateTime Start { get; init; }
    public required DateTime End { get; init; }
    public required bool IsActive { get; init; }
}