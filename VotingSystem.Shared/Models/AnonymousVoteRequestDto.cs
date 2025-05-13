using System.ComponentModel.DataAnnotations;

namespace VotingSystem.Shared.Models;

public record AnonymousVoteRequestDto
{
    public required string UserId { get; set; }
    
    public required int VoteId { get; set; }
    
    [MaxLength(1000, ErrorMessage = "Option can't be longer than 1000 characters!")]
    public required string SelectedOption { get; set; }
}