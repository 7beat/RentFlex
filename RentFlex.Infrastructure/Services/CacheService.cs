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
    public CacheService(IConnectionMultiplexer muxer, IUnitOfWork unitOfWork)
    {
        redis = muxer.GetDatabase();
        this.unitOfWork = unitOfWork;
    }

    public async Task<ApplicationStatsDto> GetOrCreateAppStatsAsync(CancellationToken cancellationToken)
    {
        var json = await redis.StringGetAsync("AppStats");

        if (string.IsNullOrEmpty(json))
        {
            var appStats = new ApplicationStatsDto(
                await unitOfWork.Users.CountAsync(cancellationToken),
                await unitOfWork.Estates.CountAsync(cancellationToken));

            json = JsonConvert.SerializeObject(appStats);

            await redis.StringSetAsync("AppStats", json);
        }

        return JsonConvert.DeserializeObject<ApplicationStatsDto>(json!)!;
        // TryCatchFinally to return serialized object?
    }

    //public async Task<T> GetOrCreateAsync<T>(CancellationToken cancellationToken)
    //{
    //    var json = await redis.StringGetAsync(nameof(T));

    //    return JsonConvert.DeserializeObject<T>(json!)!;
    //}

}
