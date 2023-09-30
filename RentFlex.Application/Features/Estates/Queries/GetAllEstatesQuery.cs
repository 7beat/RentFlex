using MediatR;
using RentFlex.Application.Contracts.Persistence;

namespace RentFlex.Application.Features.Estates.Queries;
public record GetAllEstatesQuery(Guid OwnerId) : IRequest<IEnumerable<EstateDto>>;

internal class GetAllEstatesQueryHandler : IRequestHandler<GetAllEstatesQuery, IEnumerable<EstateDto>>
{
    private readonly IUnitOfWork unitOfWork;

    public GetAllEstatesQueryHandler(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<EstateDto>> Handle(GetAllEstatesQuery request, CancellationToken cancellationToken)
    {
        var estates = await unitOfWork.Estates.FindAllAsync(cancellationToken);

        return new List<EstateDto>(); // Add mapping!
    }
}