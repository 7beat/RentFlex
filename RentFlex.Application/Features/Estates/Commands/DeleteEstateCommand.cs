using MediatR;
using RentFlex.Application.Contracts.Infrastructure.Services;
using RentFlex.Application.Contracts.Persistence;

namespace RentFlex.Application.Features.Estates.Commands;
public record DeleteEstateCommand(Guid Id) : IRequest<bool>;

internal class DeleteEstateCommandHandler : IRequestHandler<DeleteEstateCommand, bool>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IStorageService storageService;

    public DeleteEstateCommandHandler(IUnitOfWork unitOfWork, IStorageService storageService)
    {
        this.unitOfWork = unitOfWork;
        this.storageService = storageService;
    }

    public async Task<bool> Handle(DeleteEstateCommand request, CancellationToken cancellationToken)
    {
        var estate = await unitOfWork.Estates.FindSingleAsync(e => e.Id == request.Id, cancellationToken);
        foreach (var imageUrl in estate!.ImageUrls)
        {
            await storageService.DeleteAsync(imageUrl, cancellationToken);
        }
        unitOfWork.Estates.Remove(estate!);

        return await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
