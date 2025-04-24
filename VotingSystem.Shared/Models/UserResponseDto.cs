namespace VotingSystem.Shared.Models;

public class UserResponseDto
{
    /// <summary>
    /// Unique identifier for the user
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public required string FirstName { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public required string LastName { get; init; }
    
    public required string UserName { get; init; }
    
    //password

    public required string Email { get; init; }
}