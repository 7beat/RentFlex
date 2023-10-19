using AutoMapper;
using MediatR;
using RentFlex.Application.Contracts.Persistence;
using RentFlex.Application.Models;

namespace RentFlex.Application.Features.Estates.Queries;
public record GetSingleEstateQuery(Guid Id) : IRequest<EstateDto>;

internal class GetSingleEstateQueryHandler : IRequestHandler<GetSingleEstateQuery, EstateDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetSingleEstateQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<EstateDto> Handle(GetSingleEstateQuery request, CancellationToken cancellationToken)
    {
        var estate = await _unitOfWork.Estates.FindSingleAsync(e => e.Id == request.Id, cancellationToken);
        return _mapper.Map<EstateDto>(estate);
    }
}