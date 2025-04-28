using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VotingSystem.DataAccess.Config;
using VotingSystem.DataAccess.Models;
using VotingSystem.DataAccess.Services;

namespace VotingSystem.DataAccess;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<EmailSettings>(config.GetSection("EmailSettings"));
        
        // Database
        var connectionString = config.GetConnectionString("DefaultConnection");
        services.AddDbContext<VotingSystemDbContext>(options => options
            .UseSqlServer(connectionString)
            .UseLazyLoadingProxies()
        );
        
        //Identity
        services.AddIdentity<User, UserRole>(options =>
            {
                // Password settings.
                options.Password.RequiredLength = 6;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<VotingSystemDbContext>()
            .AddDefaultTokenProviders();

        // Services
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IVotesService, VotesService>();
        services.AddScoped<IAnonymousVoteService, AnonymousVoteService>();
        services.AddScoped<IVoteParticipationService, VoteParticipationService>();

        return services;
    }
}