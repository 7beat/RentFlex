using RentFlex.Domain.Entities;
using System.Linq.Expressions;

namespace RentFlex.Application.Contracts.Persistence.IRepositories;
public interface IUserRepository
{
    Task<ApplicationUser?> FindSingleAsync(Expression<Func<ApplicationUser, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApplicationUser>> FindAllAsync(CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task AddAsync(ApplicationUser user, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken = default);
}
