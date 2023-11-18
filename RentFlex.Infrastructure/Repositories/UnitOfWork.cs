using RentFlex.Application.Contracts.Persistence;
using RentFlex.Application.Contracts.Persistence.IRepositories;
using RentFlex.Infrastructure.Data;

namespace RentFlex.Infrastructure.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private bool _disposed = false;
    private readonly ApplicationDbContext dbContext;

    private IEstateRepository _estateRepository;
    private IUserRepository _userRepository;

    public IEstateRepository Estates => _estateRepository ??= new EstateRepository(dbContext);
    public IUserRepository Users => _userRepository ??= new UserRepository(dbContext);

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default!)
    {
        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!this._disposed && disposing)
        {
            dbContext.Dispose();
        }
        this._disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
