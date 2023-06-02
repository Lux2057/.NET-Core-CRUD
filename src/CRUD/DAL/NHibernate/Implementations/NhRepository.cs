namespace CRUD.DAL.NHibernate;

#region << Using >>

using CRUD.DAL.Abstractions;
using CRUD.Extensions;
using global::NHibernate;
using LinqSpecs;

#endregion

public class NhRepository : IRepository
{
    #region Properties

    private readonly ISession _session;

    #endregion

    #region Constructors

    public NhRepository(ISession session)
    {
        this._session = session;
    }

    #endregion

    #region Interface Implementations

    public async Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, new()
    {
        if (entity == null)
            return;

        await this._session.SaveAsync(entity, cancellationToken);
    }

    public async Task AddAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class, new()
    {
        var entitiesArray = entities.Where(r => r != null).ToArrayOrEmpty();

        if (!entitiesArray.Any())
            return;

        foreach (var entity in entitiesArray)
            await this._session.SaveAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, new()
    {
        if (entity == null)
            return;

        await this._session.UpdateAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class, new()
    {
        var entitiesArray = entities.Where(r => r != null).ToArrayOrEmpty();

        if (!entitiesArray.Any())
            return;

        foreach (var entity in entitiesArray)
            await this._session.UpdateAsync(entity, cancellationToken);
    }

    public async Task DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, new()
    {
        if (entity == null)
            return;

        await this._session.DeleteAsync(entity, cancellationToken);
    }

    public async Task DeleteAsync<TEntity>(TEntity[] entities, CancellationToken cancellationToken = default) where TEntity : class, new()
    {
        var entitiesArray = entities.Where(r => r != null).ToArrayOrEmpty();

        if (!entitiesArray.Any())
            return;

        foreach (var entity in entitiesArray)
            await this._session.DeleteAsync(entity, cancellationToken);
    }

    #endregion

    public IQueryable<TEntity> Get<TEntity>(Specification<TEntity> specification = default,
                                            IEnumerable<OrderSpecification<TEntity>> orderSpecifications = default)
            where TEntity : class, new()
    {
        var queryable = this._session.Query<TEntity>();

        return (specification == null ? queryable : queryable.Where(specification)).ApplyOrderSpecifications(orderSpecifications);
    }
}