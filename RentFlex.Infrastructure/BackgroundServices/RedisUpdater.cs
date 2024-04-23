using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RentFlex.Application.Contracts.Persistence;
using RentFlex.Application.Models;
using StackExchange.Redis;

namespace RentFlex.Infrastructure.BackgroundServices;
public class RedisUpdater(IConnectionMultiplexer muxer, IServiceScopeFactory scopeFactory, ILogger<RedisUpdater> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                IDatabase redisDb = muxer.GetDatabase();

                using var scope = scopeFactory.CreateScope();

                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var appStats = new ApplicationStatsDto(
                        await unitOfWork.Users.CountAsync(stoppingToken),
                        await unitOfWork.Estates.CountAsync(stoppingToken));

                var json = JsonConvert.SerializeObject(appStats);

                await redisDb.StringSetAsync("AppStats", json);

                logger.LogInformation($"Redis AppStats updated at: {DateTime.Now}");

                await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogWarning("An error occurred while updating AppStats: " + ex.Message);
            }
        }
    }
}
