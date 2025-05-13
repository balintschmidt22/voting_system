using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VotingSystem.DataAccess.Models;

namespace VotingSystem.DataAccess;

public static class DbInitializer
{
    private static async Task SeedRolesAsync(RoleManager<UserRole> roleManager)
    {
        string[] roleNames = ["Admin"]; 

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                // Create the roles and seed them to the database
                await roleManager.CreateAsync(new UserRole(roleName));
            }
        }
    }
    
    private static async Task SeedUsersAsync(UserManager<User> userManager)
    {
        var user1 = await userManager.FindByEmailAsync("johndoe@gmail.com");
        if (user1 == null)
        {
            user1 = new User { UserName = "johndoe", Email = "johndoe@gmail.com", FirstName = "John", LastName = "Doe"};

            var result = await userManager.CreateAsync(user1, "Password.123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user1, "Admin");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"User creation error: {error.Description}");
                }
            }
        }
        
        var user2 = await userManager.FindByEmailAsync("user2@gmail.com");
        if (user2 == null)
        {
            user2 = new User { UserName = "user2", Email = "user2@gmail.com", FirstName = "User", LastName = "Two"};
            var result = await userManager.CreateAsync(user2, "Password.123");
            
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user2, "Admin");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"User creation error: {error.Description}");
                }
            }
        }
        
        var user3 = await userManager.FindByEmailAsync("test@gmail.com");
        if (user3 == null)
        {
            user3 = new User { UserName = "teszt_elek", Email = "teszt_elek@gmail.com", FirstName = "Teszt", LastName = "Elek"};
            var result = await userManager.CreateAsync(user3, "Password.123");
            
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user3, "Admin");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"User creation error: {error.Description}");
                }
            }
        }
        
        const string adminEmail = "admin@example.com";
        const string adminPassword = "Admin@123";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new User { UserName = "admin", Email = adminEmail, FirstName = "Test", LastName = "Admin" };
            var result = await userManager.CreateAsync(adminUser, adminPassword);
            
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"User creation error: {error.Description}");
                }
            }
        }
    }
    
    
    public static async Task Initialize(VotingSystemDbContext context, RoleManager<UserRole>? roleManager = null, UserManager<User>? userManager = null)
    {
        await context.Database.MigrateAsync();
        
        if (roleManager != null)
        {
            SeedRolesAsync(roleManager).Wait();
        }

        if (userManager != null)
        {
            SeedUsersAsync(userManager).Wait();
        }
        
        // Check if any votes already exist
        if (context.Votes.Any())
        {
            return;
        }

        if (context.AnonymousVotes.Any())
        {
            return;
        }

        if (context.VoteParticipations.Any())
        {
            return;
        }
        
        var user1 = await userManager?.FindByEmailAsync("johndoe@gmail.com")!;
        var user2 = await userManager.FindByEmailAsync("user2@gmail.com");
        var user3 = await userManager.FindByEmailAsync("teszt_elek@gmail.com");
        var user4 = await userManager.FindByEmailAsync("admin@example.com");

        if (user1 != null && user2 != null && user3 != null && user4 != null)
        {
            Vote[] votes =
            [
                new Vote
                {
                    UserId = user1.Id,
                    Question = "What is 1+1?",
                    Options = ["2", "3"],
                    Start = DateTime.Now - TimeSpan.FromHours(4),
                    End = DateTime.Now + TimeSpan.FromDays(40)
                },
                new Vote
                {
                    UserId = user2.Id,
                    Question = "What is the meaning of life?",
                    Options = ["IDK", "42", "Nothing"],
                    Start = DateTime.Now - TimeSpan.FromHours(10),
                    End = DateTime.Now - TimeSpan.FromHours(2)
                },
                new Vote
                {
                    UserId = user1.Id,
                    Question = "What is the capital of the USA?",
                    Options = ["New York", "Los Angeles", "Washington DC", "Chicago"],
                    Start = DateTime.Now - TimeSpan.FromHours(10),
                    End = DateTime.Now + TimeSpan.FromHours(10)
                },
                new Vote
                {
                    UserId = user3.Id,
                    Question = "What is the best web-development framework?",
                    Options = ["React", "Laravel", "ASP.NET", "Angular", "Symfony", "None of the above"],
                    Start = DateTime.Now - TimeSpan.FromHours(10),
                    End = DateTime.Now + TimeSpan.FromDays(10)
                }
            ];
        
            await context.Votes.AddRangeAsync(votes);
            await context.SaveChangesAsync();

            AnonymousVote[] anonymousVotes = [
                new AnonymousVote
                {
                    VoteId = 1,
                    SelectedOption = "2"
                },
                new AnonymousVote
                {
                    VoteId = 3,
                    SelectedOption = "New York"
                },
                new AnonymousVote
                {
                    VoteId = 3,
                    SelectedOption = "Washington DC"
                },
                new AnonymousVote
                {
                    VoteId = 2,
                    SelectedOption = "Nothing"
                },
                new AnonymousVote
                {
                    VoteId = 2,
                    SelectedOption = "42"
                },
                new AnonymousVote
                {
                    VoteId = 1,
                    SelectedOption = "2"
                },
                new AnonymousVote
                {
                    VoteId = 1,
                    SelectedOption = "3"
                },
                new AnonymousVote
                {
                    VoteId = 4,
                    SelectedOption = "ASP.NET"
                },
                new AnonymousVote
                {
                    VoteId = 4,
                    SelectedOption = "Laravel"
                },
                new AnonymousVote
                {
                    VoteId = 4,
                    SelectedOption = "Laravel"
                },
                new AnonymousVote
                {
                    VoteId = 4,
                    SelectedOption = "None of the above"
                },
                new AnonymousVote
                {
                    VoteId = 2,
                    SelectedOption = "42"
                },
                new AnonymousVote
                {
                    VoteId = 2,
                    SelectedOption = "IDK"
                }
            ];
            
            await context.AnonymousVotes.AddRangeAsync(anonymousVotes);

            VoteParticipation[] voteParticipations =
            [
                              new VoteParticipation
                              {
                                  UserId = user1.Id,
                                  VoteId = 1,
                              },
                              new VoteParticipation
                              {
                                  UserId = user2.Id,
                                  VoteId = 1,
                              },
                              new VoteParticipation
                              {
                                  UserId = user3.Id,
                                  VoteId = 1,
                              },
                              new VoteParticipation
                              {
                                  UserId = user1.Id,
                                  VoteId = 2,
                              },
                              new VoteParticipation
                              {
                                  UserId = user2.Id,
                                  VoteId = 2,
                              },
                              new VoteParticipation
                              {
                                  UserId = user3.Id,
                                  VoteId = 2,
                              },
                              new VoteParticipation
                              {
                                  UserId = user4.Id,
                                  VoteId = 2,
                              },
                              new VoteParticipation
                              {
                                  UserId = user1.Id,
                                  VoteId = 3,
                              },
                              new VoteParticipation
                              {
                                  UserId = user2.Id,
                                  VoteId = 3,
                              },
                              new VoteParticipation
                              {
                                  UserId = user1.Id,
                                  VoteId = 4,
                              },
                              new VoteParticipation
                              {
                                  UserId = user2.Id,
                                  VoteId = 4,
                              },
                              new VoteParticipation
                              {
                                  UserId = user3.Id,
                                  VoteId = 4,
                              },
                              new VoteParticipation
                              {
                                  UserId = user4.Id,
                                  VoteId = 4,
                              },
                          ];

            await context.VoteParticipations.AddRangeAsync(voteParticipations);
        }
        
        await context.SaveChangesAsync();
    }
}