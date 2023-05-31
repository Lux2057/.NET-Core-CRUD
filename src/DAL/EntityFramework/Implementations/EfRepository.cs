namespace CRUD.DAL;

#region << Using >>

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CRUD.Extensions;
using LinqSpecs;
using Microsoft.EntityFrameworkCore;

#endregion

public class EfRepository : IRepository
{
    #region Properties

    protected readonly IEfDbContext _context;

    #endregion

    #region Constructors

    public EfRepository(IEfDbContext context)
    {
        this._context = context;
    }

    #endregion

    #region Interface Implementations

    public IQueryable<TEntity> Get<TEntity>(Specification<TEntity> specification = default,
                                            IEnumerable<OrderSpecification<TEntity>> orderSpecifications = default)
            where TEntity : class, new()
    {
        var dbSet = this._context.Set<TEntity>();

        return (specification == null ? dbSet : dbSet.Where(specification)).ApplyOrderSpecifications(orderSpecifications).AsNoTracking();
    }

    public async Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, new()
    {
        if (entity == null)
            return;

        _dbSet<TEntity>().Add(entity);

        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class, new()
    {
        var entitiesArray = entities.Where(r => r != null).ToArrayOrEmpty();

        if (!entitiesArray.Any())
            return;

        await _dbSet<TEntity>().AddRangeAsync(entitiesArray, cancellationToken);

        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, new()
    {
        if (entity == null)
            return;

        _dbSet<TEntity>().Update(entity);

        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class, new()
    {
        var entitiesArray = entities.Where(r => r != null).ToArrayOrEmpty();

        if (!entitiesArray.Any())
            return;

        _dbSet<TEntity>().UpdateRange(entitiesArray);

        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, new()
    {
        if (entity == null)
            return;

        _dbSet<TEntity>().Remove(entity);

        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync<TEntity>(TEntity[] entities, CancellationToken cancellationToken = default) where TEntity : class, new()
    {
        var entitiesArray = entities.Where(r => r != null).ToArrayOrEmpty();

        if (!entitiesArray.Any())
            return;

        _dbSet<TEntity>().RemoveRange(entitiesArray);

        await this._context.SaveChangesAsync(cancellationToken);
    }

    #endregion

    private DbSet<TEntity> _dbSet<TEntity>() where TEntity : class, new()
    {
        //Force tracking disable
        this._context.ChangeTracker.Clear();

        return this._context.Set<TEntity>();
    }
}