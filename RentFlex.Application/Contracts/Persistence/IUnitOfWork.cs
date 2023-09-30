using RentFlex.Application.Contracts.Persistence.IRepositories;

namespace RentFlex.Application.Contracts.Persistence;
internal interface IUnitOfWork
{
    IEstateRepository Estates { get; }
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}
