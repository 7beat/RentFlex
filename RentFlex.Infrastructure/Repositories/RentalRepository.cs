using RentFlex.Application.Contracts.Persistence.IRepositories;
using RentFlex.Domain.entities;
using RentFlex.Infrastructure.Data;

namespace RentFlex.Infrastructure.Repositories;
internal class RentalRepository : GenericRepository<Rental>, IRentalRepository
{
    private ApplicationDbContext _dbContext;

    public RentalRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public void Update(Rental rental)
    {
        _dbContext.Rentals.Update(rental);
    }
}
