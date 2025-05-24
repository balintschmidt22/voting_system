namespace VotingSystem.Blazor.WebAssembly.ViewModels;

public class UserViewModel
{
    public required string Id { get; init; }
    
    public required string FirstName { get; init; }
    
    public required string LastName { get; init; }
    
    public required string UserName { get; init; }
    
    public required string Email { get; init; }
}