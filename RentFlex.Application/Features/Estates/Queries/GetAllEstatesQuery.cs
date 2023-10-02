using AutoMapper;
using MediatR;
using RentFlex.Application.Contracts.Persistence;
using RentFlex.Application.Models;

namespace RentFlex.Application.Features.Estates.Queries;
public record GetAllEstatesQuery(Guid OwnerId) : IRequest<IEnumerable<EstateDto>>;

internal class GetAllEstatesQueryHandler : IRequestHandler<GetAllEstatesQuery, IEnumerable<EstateDto>>
{
    private readonly IMapper mapper;
    private readonly IUnitOfWork unitOfWork;

    public GetAllEstatesQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        this.mapper = mapper;
        this.unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<EstateDto>> Handle(GetAllEstatesQuery request, CancellationToken cancellationToken)
    {
        var estates = await unitOfWork.Estates.FindAllAsync(e => e.OwnerId == request.OwnerId, cancellationToken);

        // AuthService will have GetUserRole and on that i will switch: Admin -> GetAll, User -> GetHis

        return mapper.Map<List<EstateDto>>(estates);
    }
}