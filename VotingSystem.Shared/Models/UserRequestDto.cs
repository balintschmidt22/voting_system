using System.ComponentModel.DataAnnotations;

namespace VotingSystem.Shared.Models;

public record UserRequestDto
{
    [StringLength(40, ErrorMessage = "First Name is too long")]
    public required string FirstName { get; init; }
    
    [StringLength(40, ErrorMessage = "Last Name is too long")]
    public required string LastName { get; init; }
    
    [StringLength(255, ErrorMessage = "Username is too long")]
    public required string UserName { get; init; }
    
    [EmailAddress(ErrorMessage = "Email is invalid")]
    public required string Email { get; init; }
    
    public required string Password { get; init; }
}
