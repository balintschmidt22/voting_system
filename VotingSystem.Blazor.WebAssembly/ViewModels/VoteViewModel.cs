using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VotingSystem.Shared.Models;

namespace VotingSystem.Blazor.WebAssembly.ViewModels;

public class VoteViewModel
{
    //required
    public UserResponseDto? User { get; set; }
    
    [Required(ErrorMessage = "Question shouldn't be empty")]
    [MinLength(5, ErrorMessage = "Question is too short")]
    [MaxLength(255, ErrorMessage = "Question is too long")]
    public string? Question { get; set; }
    
    [Required(ErrorMessage = "Options shouldn't be empty")]
    [MinLength(5, ErrorMessage = "Enter at least two options separated by ';'")]
    [MaxLength(5000, ErrorMessage = "Too much text")]
    public string OptionsRaw { get; set; } = string.Empty;

    [JsonIgnore]
    public string[] Options => OptionsRaw
                                   ?.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                               ?? Array.Empty<string>();


    [Required(ErrorMessage = "Start shouldn't be empty")]
    [CustomValidation(typeof(VoteViewModel), nameof(ValidateStart))]
    public DateTime? Start { get; set; }
    
    [Required(ErrorMessage = "End shouldn't be empty")]
    [CustomValidation(typeof(VoteViewModel), nameof(ValidateEnd))]
    public DateTime? End { get; set; }
    
    public static ValidationResult? ValidateStart(DateTime date, ValidationContext context)
    {
        return (date > DateTime.Now)
            ? ValidationResult.Success
            : new ValidationResult("The start date must be in the future.", new[] { nameof(VoteViewModel.Start) });
    }
    
    public static ValidationResult? ValidateEnd(DateTime date, ValidationContext context)
    {
        return (date > DateTime.Now)
            ? ValidationResult.Success
            : new ValidationResult("The end date must be in the future.", new[] { nameof(VoteViewModel.End) });
    }
    
    //end > start + 15

    /*public static ValidationResult? ValidateUserId(int id, ValidationContext context)
    {
        return (id > 0)
            ? ValidationResult.Success
            : new ValidationResult("Please select a room.", new[] { nameof(ScreeningViewModel.Room) });
    }*/
}