using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VotingSystem.DataAccess.Models;

public class VoteParticipation
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public string UserId { get; set; } = null!;
    public virtual User User { get; set; } = null!;

    [ForeignKey("Vote")] 
    public int VoteId { get; set; }
    public virtual Vote Vote { get; set; } = null!;

    public DateTime VotedAt { get; set; } = DateTime.UtcNow;
}