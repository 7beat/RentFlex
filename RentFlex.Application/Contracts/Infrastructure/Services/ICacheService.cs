using RentFlex.Application.Models;

namespace RentFlex.Application.Contracts.Infrastructure.Services;
public interface ICacheService
{
    Task<ApplicationStatsDto> GetAppStatsAsync(CancellationToken cancellationToken);
}
