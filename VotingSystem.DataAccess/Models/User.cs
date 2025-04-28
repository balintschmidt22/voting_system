using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace VotingSystem.DataAccess.Models;

public class User : IdentityUser
{
    [MaxLength(40)] 
    public string FirstName { get; set; } = null!;
    
    [MaxLength(40)]
    public string LastName { get; set; } = null!;
    
    public Guid RefreshToken { get; set; } = Guid.NewGuid();
    
    public virtual ICollection<Vote> Votes { get; set; } = [];
    
    public virtual ICollection<VoteParticipation> VoteParticipations { get; set; } = [];

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}