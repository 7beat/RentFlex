using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RentFlex.Application.Contracts.Infrastructure.Services;
using RentFlex.Application.Contracts.Persistence;
using RentFlex.Application.Models;
using StackExchange.Redis;

namespace RentFlex.Infrastructure.Services;
public class CacheService : ICacheService
{
    private readonly IDatabase redis;
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<CacheService> logger;
    public CacheService(IConnectionMultiplexer muxer, IUnitOfWork unitOfWork, ILogger<CacheService> logger)
    {
        redis = muxer.GetDatabase();
        this.unitOfWork = unitOfWork;
        this.logger = logger;
    }

    public async Task<ApplicationStatsDto> GetOrCreateAppStatsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var json = await redis.StringGetAsync("AppStats");

            if (string.IsNullOrEmpty(json))
            {
                var appStats = new ApplicationStatsDto(
                    await unitOfWork.Users.CountAsync(cancellationToken),
                    await unitOfWork.Estates.CountAsync(cancellationToken));

                json = JsonConvert.SerializeObject(appStats);

                await redis.StringSetAsync("AppStats", json);

                return appStats;
            }
            else
            {
                return JsonConvert.DeserializeObject<ApplicationStatsDto>(json!)!;
            }
        }
        catch (Exception e)
        {
            logger.LogWarning($"Exception occurred while fetching data from Redis, message: {e.Message}");
        }

        return new ApplicationStatsDto(
                await unitOfWork.Users.CountAsync(cancellationToken),
                await unitOfWork.Estates.CountAsync(cancellationToken));

    }

    //public async Task<T> GetOrCreateAsync<T>(CancellationToken cancellationToken)
    //{
    //    var json = await redis.StringGetAsync(nameof(T));

    //    return JsonConvert.DeserializeObject<T>(json!)!;
    //}

}
