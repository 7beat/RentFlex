using Azure.Identity;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using RentFlex.Application.Contracts.Infrastructure.Services;
using RentFlex.Application.Contracts.Persistence;
using RentFlex.Domain.Entities;
using RentFlex.Infrastructure.BackgroundServices;
using RentFlex.Infrastructure.Data;
using RentFlex.Infrastructure.Repositories;
using RentFlex.Infrastructure.Services;
using RentFlex.Utility.WireMock;
using StackExchange.Redis;

namespace RentFlex.Infrastructure;
public static class InfrastructureServicesRegistration
{
    public static void RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureDbContext(configuration);
        //services.ConfigureIdentity();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.ConfigureServices();
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configuration.GetConnectionString("Cache")!));
        services.ConfigureCache(configuration);
        services.AddHostedService<RedisUpdater>();

        // AzureAD Identity
        services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(configuration);

        services.AddSingleton<GraphServiceClient>(sp =>
        {
            var tenantId = configuration["AzureAd:TenantId"];
            var clientId = configuration["AzureAd:ClientId"];
            var clientSecret = configuration["AzureAd:ClientSecret"];

            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            return new(clientSecretCredential);
        });

        services.AddScoped<IGraphService, GraphService>();

        services.AddHttpClient("WireMockClient", client =>
        {
            client.BaseAddress = new Uri("http://localhost:5000");
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
        //services.AddTransient<IAuthService, AuthService>();
        services.AddScoped<IAirbnbService, AirbnbService>();
        services.AddScoped<ICacheService, CacheService>();
    }

    private static void ConfigureCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Cache");
        });
    }
}
