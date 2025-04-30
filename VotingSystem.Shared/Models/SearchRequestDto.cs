using System.ComponentModel.DataAnnotations;

namespace VotingSystem.Shared.Models;

public record SearchRequestDto
{
    [StringLength(255, ErrorMessage = "Search is too long")]
    public required string Sub { get; init; }
    public required bool IsActive { get; init; }
}