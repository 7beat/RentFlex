using AutoMapper;
using MediatR;
using RentFlex.Application.Contracts.Persistence;
using RentFlex.Application.Features.Users.Commands;
using RentFlex.Application.Models;

namespace RentFlex.Application.Features.Estates.Queries;
public record GetAllEstatesQuery(Guid OwnerId) : IRequest<IEnumerable<EstateDto>>;

internal class GetAllEstatesQueryHandler : IRequestHandler<GetAllEstatesQuery, IEnumerable<EstateDto>>
{
    private readonly IMapper mapper;
    private readonly IUnitOfWork unitOfWork;
    private readonly IMediator mediator;

    public GetAllEstatesQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IMediator mediator)
    {
        this.mapper = mapper;
        this.unitOfWork = unitOfWork;
        this.mediator = mediator;
    }

    public async Task<IEnumerable<EstateDto>> Handle(GetAllEstatesQuery request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.FindSingleAsync(u => u.Id == request.OwnerId, cancellationToken);

        if (user is null)
            await mediator.Publish(new CreateUserNotification(request.OwnerId));

        return user!.Estates.Any() ? mapper.Map<List<EstateDto>>(user!.Estates) : Enumerable.Empty<EstateDto>();
    }
}