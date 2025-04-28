using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VotingSystem.DataAccess.Models;

public class AnonymousVote
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Vote")]
    public int VoteId { get; set; }
    public virtual Vote Vote { get; set; } = null!;

    public string SelectedOption { get; set; } = null!;

    public DateTime SubmittedAt { get; init; } = DateTime.UtcNow;
}