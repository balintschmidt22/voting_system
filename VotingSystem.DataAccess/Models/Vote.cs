using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VotingSystem.DataAccess.Models;

public class Vote
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public string UserId { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    
    [MinLength(5)]
    [MaxLength(255)]
    public string Question { get; set; } = null!;
    
    [MinLength(2)]
    [MaxLength(100)]
    public string[] Options { get; set; } = null!;

    public required DateTime Start { get; set; }
    
    public required DateTime End { get; set; }
    
    public virtual ICollection<AnonymousVote> AnonymousVotes { get; set; } = [];
    public virtual ICollection<VoteParticipation> VoteParticipations { get; set; } = [];

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}