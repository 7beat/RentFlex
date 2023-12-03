using RentFlex.Domain.common;
using System.Linq.Expressions;

namespace RentFlex.Application.Contracts.Persistence.IRepositories;
public interface IGenericRepository<TEntity> where TEntity : EntityBase
{
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity?> FindSingleAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> FindAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    void Remove(TEntity entity);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
}
