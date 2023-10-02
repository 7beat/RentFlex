using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentFlex.Application.Contracts.Persistence;
using RentFlex.Infrastructure.Data;
using RentFlex.Infrastructure.Identity;
using RentFlex.Infrastructure.Repositories;

namespace RentFlex.Infrastructure;
public static class InfrastructureServicesRegistration
{
    public static void RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureDbContext(configuration);
        services.ConfigureIdentity();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqloptions =>
            {
                sqloptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            });
        });
    }

    private static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddDefaultIdentity<ApplicationUser>(o =>
        {
            o.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
    }
}
