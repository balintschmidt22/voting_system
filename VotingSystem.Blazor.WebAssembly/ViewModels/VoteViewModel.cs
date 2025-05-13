using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VotingSystem.Shared.Models;

namespace VotingSystem.Blazor.WebAssembly.ViewModels;

public class VoteViewModel
{
    public int? Id { get; set; }
    public string? UserId { get; set; }
    
    [Required(ErrorMessage = "Question shouldn't be empty")]
    [MinLength(5, ErrorMessage = "Question is too short")]
    [MaxLength(255, ErrorMessage = "Question is too long")]
    public string? Question { get; set; }
    
    [Required(ErrorMessage = "Options shouldn't be empty")]
    [MinLength(3, ErrorMessage = "Enter at least two options separated by ';'")]
    [MaxLength(5000, ErrorMessage = "Too much text")]
    public string OptionsRaw { get; set; } = string.Empty;

    [JsonIgnore]
    public string[] Options => OptionsRaw
                                   ?.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                               ?? Array.Empty<string>();


    public DateTime? Start { get; set; }
    
    public DateTime? End { get; set; }
    
    public ICollection<VoteParticipationResponseDto>? VoteParticipations { get; set; }
}