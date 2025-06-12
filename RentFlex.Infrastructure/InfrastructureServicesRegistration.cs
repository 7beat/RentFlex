using System.Security.Claims;
using Azure.Identity;
using Azure.Storage.Blobs;
using MediatR;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using RentFlex.Application.Contracts.Infrastructure.Services;
using RentFlex.Application.Contracts.Persistence;
using RentFlex.Application.Features.Users.Commands;
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
        services.AddFeatureManagement(configuration.GetSection("FeatureFlags"));

        services.ConfigureDbContext(configuration);
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.ConfigureServices();
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configuration.GetConnectionString("Cache")!));
        services.ConfigureCache(configuration);
        services.AddHostedService<RedisUpdater>();
        services.AddHostedService<DbSnapshotExporter>();

        // AzureAD Identity
        services.ConfigureIdentity(configuration);

        services.AddSingleton<GraphServiceClient>(sp =>
        {
            var tenantId = configuration["AzureAd:TenantId"];
            var clientId = configuration["AzureAd:ClientId"];
            var clientSecret = configuration["AzureAd:ClientSecret"];

            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            return new(clientSecretCredential);
        });

        services.AddScoped<IGraphService, GraphService>();

        var blobConnectionString = configuration.GetConnectionString("AzureStorage");
        services.AddSingleton(new BlobServiceClient(blobConnectionString));

        services.AddHttpClient("WireMockClient", client =>
        {
            client.BaseAddress = new Uri("http://localhost:5000");
        }).AddStandardResilienceHandler();

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

    private static void ConfigureServices(this IServiceCollection services)
    {
        //services.AddTransient<IAuthService, AuthService>();
        services.AddScoped<IAirbnbService, AirbnbService>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IStorageService, StorageService>();
    }

    private static void ConfigureCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Cache");
        });
    }

    private static void ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(configuration);

        services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
            options.Events = new OpenIdConnectEvents
            {
                OnTokenValidated = async context =>
                {
                    var userId = context.Principal!.FindFirstValue(ClaimTypes.NameIdentifier);

                    var unitOfWork = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();
                    var mediator = context.HttpContext.RequestServices.GetRequiredService<IMediator>();

                    if (!await unitOfWork.Users.ExistsAsync(Guid.Parse(userId!)))
                        await mediator.Publish(new CreateUserNotification(Guid.Parse(userId!)));
                }
            };
        });
    }
}
