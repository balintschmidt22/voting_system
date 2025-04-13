using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VotingSystem.DataAccess;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration config)
    {
       
        // Database
        var connectionString = config.GetConnectionString("DefaultConnection");
        services.AddDbContext<VotingSystemDbContext>(options => options
            .UseSqlServer(connectionString)
            .UseLazyLoadingProxies()
        );

        return services;
    }
}