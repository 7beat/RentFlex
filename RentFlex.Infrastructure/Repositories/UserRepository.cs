using Microsoft.EntityFrameworkCore;
using RentFlex.Application.Contracts.Persistence.IRepositories;
using RentFlex.Domain.Entities;
using RentFlex.Infrastructure.Data;
using System.Linq.Expressions;

namespace RentFlex.Infrastructure.Repositories;
public class UserRepository : IUserRepository
{
    private ApplicationDbContext _dbContext;
    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ApplicationUser>> FindAllAsync(CancellationToken cancellationToken = default) =>
        await _dbContext.Set<ApplicationUser>().Include(u => u.Estates).ToListAsync(cancellationToken);

    public async Task<ApplicationUser?> FindSingleAsync(Expression<Func<ApplicationUser, bool>> predicate, CancellationToken cancellationToken = default) =>
        await _dbContext.Set<ApplicationUser>().Include(u => u.Estates).FirstOrDefaultAsync(predicate, cancellationToken);

}
