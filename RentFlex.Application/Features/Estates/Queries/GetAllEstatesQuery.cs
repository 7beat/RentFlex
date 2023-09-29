using MediatR;

namespace RentFlex.Application.Features.Estates.Queries;
public record GetAllEstatesQuery(Guid OwnerId) : IRequest<IEnumerable<EstateDto>>;

internal class GetAllEstatesQueryHandler : IRequestHandler<GetAllEstatesQuery, IEnumerable<EstateDto>>
{
    public async Task<IEnumerable<EstateDto>> Handle(GetAllEstatesQuery request, CancellationToken cancellationToken)
    {
        return new List<EstateDto>(); // ToDo: Implement
    }
}