using System.ComponentModel.DataAnnotations;

namespace VotingSystem.Shared.Models;

/// <summary>
/// UserRequestDTO
/// </summary>
public record UserRequestDto
{
    [StringLength(40, ErrorMessage = "First Name is too long")]
    public required string FirstName { get; init; }
    
    [StringLength(40, ErrorMessage = "Last Name is too long")]
    public required string LastName { get; init; }
    
    /// <summary>
    /// Name of the person making the reservation
    /// </summary>
    [StringLength(255, ErrorMessage = "Username is too long")]
    public required string UserName { get; init; }
    

    /// <summary>
    /// Email of the person making the reservation
    /// </summary>
    [EmailAddress(ErrorMessage = "Email is invalid")]
    public required string Email { get; init; }
    
    /// <summary>
    /// Password of the person making the reservation
    /// </summary>
    public required string Password { get; init; }
}
