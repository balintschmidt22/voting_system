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
            var result = await userManager.CreateAsync(user2, "Password_2");
            
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
            user3 = new User { UserName = "teszt_elek", Email = "test@gmail.com", FirstName = "Teszt", LastName = "Elek"};
            var result = await userManager.CreateAsync(user3, "testPW@123");
            
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


        // Check if any movies already exist
        if (context.Votes.Any())
        {
            return;
        }

/*        User[] users =
        [
            new User
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "jd",
                Email = "john.doe@gmail.com"
            },
            new User
            {
                FirstName = "Jonathan",
                LastName = "Doe",
                UserName = "jd1",
                Email = "john.doe1@gmail.com"
            },
            new User
            {
                FirstName = "Johnnie",
                LastName = "Doe",
                UserName = "jd2",
                Email = "john.doe2@gmail.com"
            }
        ];

        context.Users.AddRange(users);*/
        
        var user = await userManager.FindByEmailAsync("johndoe@gmail.com");
        var user2 = await userManager.FindByEmailAsync("user2@gmail.com");

        if (user != null && user2 != null)
        {
            Vote[] votes =
            [
                new Vote
                {
                    UserId = user.Id,
                    Question = "What is 1+1?",
                    Options = ["2", "3"],
                    Start = DateTime.Now - TimeSpan.FromHours(1),
                    End = DateTime.Now + TimeSpan.FromHours(1)
                },
                new Vote
                {
                    UserId = user2.Id,
                    Question = "What is the meaning of life?",
                    Options = ["IDK", "42", "Nothing"],
                    Start = DateTime.Now - TimeSpan.FromHours(1),
                    End = DateTime.Now + TimeSpan.FromHours(1)
                },
                new Vote
                {
                    UserId = user.Id,
                    Question = "What is the capital of the USA?",
                    Options = ["New York", "Los Angeles", "Washington DC", "Chicago"],
                    Start = DateTime.Now - TimeSpan.FromHours(1),
                    End = DateTime.Now + TimeSpan.FromHours(1)
                }
            ];
        
            context.Votes.AddRange(votes);
        }
        
        await context.SaveChangesAsync();
    }
}