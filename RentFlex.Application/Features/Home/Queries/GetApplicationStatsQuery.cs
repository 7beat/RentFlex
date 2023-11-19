using MediatR;
using RentFlex.Application.Contracts.Persistence;
using RentFlex.Application.Models;

namespace RentFlex.Application.Features.Home.Queries;
public record GetApplicationStatsQuery : IRequest<ApplicationStatsDto>
{
    internal class GetApplicationStatsQueryHandler : IRequestHandler<GetApplicationStatsQuery, ApplicationStatsDto>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetApplicationStatsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<ApplicationStatsDto> Handle(GetApplicationStatsQuery request, CancellationToken cancellationToken) =>
            new(
                await unitOfWork.Users.CountAsync(cancellationToken),
                await unitOfWork.Estates.CountAsync(cancellationToken)
                );

    }
}
