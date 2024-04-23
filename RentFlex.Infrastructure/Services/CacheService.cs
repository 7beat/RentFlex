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

    public async Task<ApplicationStatsDto> GetAppStatsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var json = await redis.StringGetAsync("AppStats");
            return JsonConvert.DeserializeObject<ApplicationStatsDto>(json!)!;
        }
        catch (Exception e)
        {
            logger.LogError($"Exception occurred while fetching data from Redis, message: {e.Message}");
        }

        return new ApplicationStatsDto(
                await unitOfWork.Users.CountAsync(cancellationToken),
                await unitOfWork.Estates.CountAsync(cancellationToken));

    }
}
