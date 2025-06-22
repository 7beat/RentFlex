using AutoMapper;
using MediatR;
using RentFlex.Application.Contracts.Infrastructure.Services;
using RentFlex.Application.Contracts.Persistence;
using RentFlex.Application.Models;

namespace RentFlex.Application.Features.Rentals.Queries;
public record GetAllRentalsQuery(string UserId) : IRequest<IEnumerable<RentalDto>>
{
    internal class GetAllRentalsQueryHandler : IRequestHandler<GetAllRentalsQuery, IEnumerable<RentalDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IAirbnbApiService airbnbApiService;

        public GetAllRentalsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IAirbnbApiService airbnbApiService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.airbnbApiService = airbnbApiService;
        }

        public async Task<IEnumerable<RentalDto>> Handle(GetAllRentalsQuery request, CancellationToken cancellationToken)
        {
            var externalRentals = await airbnbApiService.GetAllRentals(Guid.Parse("592fdf9f-2395-4a12-8f66-1e8b3b53b6fc"), Guid.Parse("9d1063e1-125e-45c6-bef3-d5baaa717152"), cancellationToken);
            var rentals = await unitOfWork.Rentals.FindAllAsync(r => r.Estate.UserId == Guid.Parse(request.UserId), cancellationToken);

            var rentalsDto = mapper.Map<IEnumerable<RentalDto>>(rentals);
            var combinedRentals = rentalsDto.Concat(externalRentals);
            return combinedRentals;
        }
    }
}

