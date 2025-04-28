using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VotingSystem.DataAccess.Models;

namespace VotingSystem.DataAccess;

public class VotingSystemDbContext : IdentityDbContext<User, UserRole, string>
{
    public DbSet<Vote> Votes { get; set; } = null!;
    public DbSet<VoteParticipation> VoteParticipations { get; set; } = null!;
    public DbSet<AnonymousVote> AnonymousVotes { get; set; } = null!;

    public VotingSystemDbContext(DbContextOptions<VotingSystemDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Vote>()
            .HasOne(v => v.User)
            .WithMany(u => u.Votes)
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<VoteParticipation>()
            .HasOne(v => v.User)
            .WithMany(u => u.VoteParticipations)
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<VoteParticipation>()
            .HasOne(v => v.Vote)
            .WithMany(v => v.VoteParticipations)
            .HasForeignKey(v => v.VoteId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<AnonymousVote>()
            .HasOne(v => v.Vote)
            .WithMany(v => v.AnonymousVotes)
            .HasForeignKey(v => v.VoteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}