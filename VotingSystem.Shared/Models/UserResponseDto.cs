namespace VotingSystem.Shared.Models;

public record UserResponseDto
{
    public required string Id { get; init; }
    
    public required string FirstName { get; init; }
    
    public required string LastName { get; init; }
    
    public required string UserName { get; init; }
    
    public required string Email { get; init; }
    
    public required string Password { get; init; }

    public DateTime CreatedAt { get; init; }
    
    public DateTime UpdatedAt { get; init; }
    
    public DateTime DeletedAt { get; init; }
}