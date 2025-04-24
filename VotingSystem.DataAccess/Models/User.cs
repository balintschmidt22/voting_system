using System.ComponentModel.DataAnnotations;

namespace VotingSystem.DataAccess.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [MaxLength(30)] public string FirstName { get; set; } = null!;
    
    [MaxLength(30)]
    public string LastName { get; set; } = null!;
    
    [MaxLength(30)]
    public string UserName { get; set; } = null!;
    
    //password

    [MaxLength(255)] public string Email { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public DateTime? DeletedAt { get; set; }
}