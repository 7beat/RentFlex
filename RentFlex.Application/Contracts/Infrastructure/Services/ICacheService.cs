using RentFlex.Application.Models;

namespace RentFlex.Application.Contracts.Infrastructure.Services;
public interface ICacheService
{
    Task<ApplicationStatsDto> GetOrCreateAppStatsAsync(CancellationToken cancellationToken);
    //Task<T> GetOrCreateAsync<T>(CancellationToken cancellationToken);
}
