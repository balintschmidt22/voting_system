using Microsoft.EntityFrameworkCore;
using VotingSystem.DataAccess.Models;

namespace VotingSystem.DataAccess;

public static class DbInitializer
{
    public static void Initialize(VotingSystemDbContext context)
    {
        context.Database.Migrate();

        if (context.Users.Any())
        {
            return;
        }

        User[] users =
        [
            new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com"
            },
            new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com"
            },
            new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com"
            }
        ];

        context.Users.AddRange(users);

        Vote[] votes =
        [
            new Vote
            {
                UserId = users[0].Id,
            },
            new Vote
            {
                UserId = users[1].Id,
            },
            new Vote
            {
                UserId = users[2].Id,    
            }
        ];
        
        context.Votes.AddRange(votes);
        
        context.SaveChanges();
    }
}