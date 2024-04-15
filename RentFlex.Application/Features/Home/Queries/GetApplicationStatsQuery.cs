using MediatR;
using RentFlex.Application.Contracts.Infrastructure.Services;
using RentFlex.Application.Models;

namespace RentFlex.Application.Features.Home.Queries;
public record GetApplicationStatsQuery : IRequest<ApplicationStatsDto>
{
    internal class GetApplicationStatsQueryHandler(ICacheService cacheService) : IRequestHandler<GetApplicationStatsQuery, ApplicationStatsDto>
    {
        public async Task<ApplicationStatsDto> Handle(GetApplicationStatsQuery request, CancellationToken cancellationToken) =>
            await cacheService.GetOrCreateAppStatsAsync(cancellationToken);

    }
}
