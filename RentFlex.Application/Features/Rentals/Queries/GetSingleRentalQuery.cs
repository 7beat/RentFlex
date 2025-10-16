using AutoMapper;
using MediatR;
using RentFlex.Application.Contracts.Persistence;
using RentFlex.Application.Models;

namespace RentFlex.Application.Features.Rentals.Queries;
public record GetSingleRentalQuery(Guid InvoiceId) : IRequest<RentalDto>;

internal class GetSingleRentalQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetSingleRentalQuery, RentalDto>
{
    public async Task<RentalDto> Handle(GetSingleRentalQuery request, CancellationToken cancellationToken)
    {
        var rental = await unitOfWork.Rentals.FindSingleAsync(r => r.Id == request.InvoiceId, cancellationToken);

        return mapper.Map<RentalDto>(rental);
    }
}
