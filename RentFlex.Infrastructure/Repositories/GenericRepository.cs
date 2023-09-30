using Microsoft.EntityFrameworkCore;
using RentFlex.Application.Contracts.Persistence.IRepositories;
using RentFlex.Domain.common;
using RentFlex.Infrastructure.Data;
using System.Linq.Expressions;

namespace RentFlex.Infrastructure.Repositories;
public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : EntityBase
{
    private readonly ApplicationDbContext _dbContext;
    internal DbSet<TEntity> dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _dbContext = context;
        this.dbSet = _dbContext.Set<TEntity>();
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public Task<IEnumerable<TEntity>> FindAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await dbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<TEntity> FindSingleAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await dbSet.SingleOrDefaultAsync(predicate, cancellationToken);
    }

    public void Remove(TEntity entity)
    {
        dbSet.Remove(entity);
    }

    private static ICollection<string> GetIncludeablePropertyNames()
    {
        var entityType = typeof(TEntity);
        var properties = entityType.GetProperties()
            .Where(p => p.PropertyType.IsClass && !p.PropertyType.FullName!.StartsWith("System.")
                     && !p.PropertyType.IsArray && typeof(EntityBase).IsAssignableFrom(p.PropertyType))
            .Select(p => p.Name)
            .ToList();

        return properties;
    }
}
