using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace VotingSystem.DataAccess.Models;

public class User : IdentityUser
{
/*    [Key]
    public new int Id { get; set; }*/

    [MaxLength(40)] 
    public string FirstName { get; set; } = null!;
    
    [MaxLength(40)]
    public string LastName { get; set; } = null!;
    
/*    [MaxLength(40)]
    public new string UserName { get; set; } = null!;*/
    
    //password

/*    [MaxLength(255)] 
    public new string Email { get; set; } = null!;*/
    
    public Guid? RefreshToken { get; set; }
    
    public virtual ICollection<Vote> Votes { get; set; } = [];
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public DateTime? DeletedAt { get; set; }
}