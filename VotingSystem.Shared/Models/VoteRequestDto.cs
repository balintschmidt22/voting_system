﻿using System.ComponentModel.DataAnnotations;

namespace VotingSystem.Shared.Models;

public record VoteRequestDto
{
    [MinLength(5)]
    [MaxLength(255)]
    public required string Question { get; init; }
    
    [MinLength(2)]
    [MaxLength(100)]
    public required string[] Options { get; init; }
    
    public required DateTime Start { get; init; }
    
    public required DateTime End { get; init; }
}