using Microsoft.EntityFrameworkCore;
using VotingSystem.DataAccess.Models;

namespace VotingSystem.DataAccess;

public class VotingSystemDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Vote> Votes { get; set; } = null!;

    public VotingSystemDbContext(DbContextOptions<VotingSystemDbContext> options)
        : base(options)
    {
    }
    
    
}