namespace CRUD.DAL.EntityFramework;

#region << Using >>

using CRUD.DAL.Abstractions;
using Extensions;
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

    public IQueryable<TEntity> Read<TEntity>(Specification<TEntity> specification = default,
                                             IEnumerable<OrderSpecification<TEntity>> orderSpecifications = default)
            where TEntity : class, new()
    {
        var dbSet = this._context.Set<TEntity>();

        return (specification == null ? dbSet : dbSet.Where(specification)).ApplyOrderSpecifications(orderSpecifications).AsNoTracking();
    }

    public async Task CreateAsync<TEntity>(TEntity entity,
                                           CancellationToken cancellationToken = default)
            where TEntity : class, new()
    {
        if (entity == null)
            return;

        dbSet<TEntity>().Add(entity);

        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task CreateAsync<TEntity>(IEnumerable<TEntity> entities,
                                           CancellationToken cancellationToken = default)
            where TEntity : class, new()
    {
        var entitiesArray = entities.Where(r => r != null).ToArrayOrEmpty();

        if (!entitiesArray.Any())
            return;

        await dbSet<TEntity>().AddRangeAsync(entitiesArray, cancellationToken);

        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync<TEntity>(TEntity entity,
                                           CancellationToken cancellationToken = default)
            where TEntity : class, new()
    {
        if (entity == null)
            return;

        dbSet<TEntity>().Update(entity);

        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync<TEntity>(IEnumerable<TEntity> entities,
                                           CancellationToken cancellationToken = default)
            where TEntity : class, new()
    {
        var entitiesArray = entities.Where(r => r != null).ToArrayOrEmpty();

        if (!entitiesArray.Any())
            return;

        dbSet<TEntity>().UpdateRange(entitiesArray);

        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync<TEntity>(TEntity entity,
                                           CancellationToken cancellationToken = default)
            where TEntity : class, new()
    {
        if (entity == null)
            return;

        dbSet<TEntity>().Remove(entity);

        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync<TEntity>(IEnumerable<TEntity> entities,
                                           CancellationToken cancellationToken = default)
            where TEntity : class, new()
    {
        var entitiesArray = entities.Where(r => r != null).ToArrayOrEmpty();

        if (!entitiesArray.Any())
            return;

        dbSet<TEntity>().RemoveRange(entitiesArray);

        await this._context.SaveChangesAsync(cancellationToken);
    }

    #endregion

    private DbSet<TEntity> dbSet<TEntity>() where TEntity : class, new()
    {
        //Force tracking disable
        this._context.ChangeTracker.Clear();

        return this._context.Set<TEntity>();
    }
}