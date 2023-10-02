using RentFlex.Application.Contracts.Persistence.IRepositories;
using RentFlex.Domain.entities;
using RentFlex.Infrastructure.Data;

namespace RentFlex.Infrastructure.Repositories;
public class EstateRepository : GenericRepository<Estate>, IEstateRepository
{
    private ApplicationDbContext _dbContext;
    public EstateRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    public void Update(Estate estate)
    {
        _dbContext.Estates.Update(estate);
    }
}
