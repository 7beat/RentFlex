using AutoMapper;
using MediatR;
using RentFlex.Application.Contracts.Persistence;
using RentFlex.Application.Models;

namespace RentFlex.Application.Features.Rentals.Queries;
public record GetAllRentalsQuery(string UserId) : IRequest<IEnumerable<RentalDto>>
{
    internal class GetAllRentalsQueryHandler : IRequestHandler<GetAllRentalsQuery, IEnumerable<RentalDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GetAllRentalsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<RentalDto>> Handle(GetAllRentalsQuery request, CancellationToken cancellationToken)
        {
            var rentals = await unitOfWork.Rentals.FindAllAsync(r => r.Estate.UserId == request.UserId);
            var rentalsDto = mapper.Map<IEnumerable<RentalDto>>(rentals);
            return rentalsDto;
        }
    }
}

