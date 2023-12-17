using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentFlex.Application.Contracts.Identity;
using RentFlex.Application.Contracts.Infrastructure.Services;
using RentFlex.Application.Contracts.Persistence;
using RentFlex.Domain.Entities;
using RentFlex.Infrastructure.Data;
using RentFlex.Infrastructure.Repositories;
using RentFlex.Infrastructure.Services;
using RentFlex.Utility.WireMock;

namespace RentFlex.Infrastructure;
public static class InfrastructureServicesRegistration
{
    public static void RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureDbContext(configuration);
        services.ConfigureIdentity();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.ConfigureServices();

        services.AddHttpClient("WireMockClient", client =>
        {
            client.BaseAddress = new Uri("http://localhost:8080");
        });

        WireMockService.Start();
        WireMockService.ConfigureEndpoints("592fdf9f-2395-4a12-8f66-1e8b3b53b6fc", "9d1063e1-125e-45c6-bef3-d5baaa717152");
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
        services.AddIdentity<ApplicationUser, IdentityRole>(o =>
        {
            o.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders()
        .AddDefaultUI();

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
        });
    }

    private static void ConfigureServices(this IServiceCollection services)
    {
        services.AddTransient<IAuthService, AuthService>();
        services.AddScoped<IAirbnbService, AirbnbService>();
    }
}
