using MediatR;
using RentFlex.Application.Contracts.Persistence;

namespace RentFlex.Application.Features.Estates.Commands;
public record DeleteEstateCommand(Guid Id) : IRequest<bool>;

internal class DeleteEstateCommandHandler : IRequestHandler<DeleteEstateCommand, bool>
{
    private readonly IUnitOfWork unitOfWork;

    public DeleteEstateCommandHandler(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteEstateCommand request, CancellationToken cancellationToken)
    {
        var estate = await unitOfWork.Estates.FindSingleAsync(e => e.Id == request.Id, cancellationToken);
        unitOfWork.Estates.Remove(estate);
        return await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
