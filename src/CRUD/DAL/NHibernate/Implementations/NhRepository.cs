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

    public async Task CreateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, new()
    {
        if (entity == null)
            return;

        this._session.Clear();

        await this._session.SaveAsync(entity, cancellationToken);
        await this._session.FlushAsync();
    }

    public async Task CreateAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class, new()
    {
        var entitiesArray = entities.Where(r => r != null).ToArrayOrEmpty();

        if (!entitiesArray.Any())
            return;

        this._session.Clear();

        foreach (var entity in entitiesArray)
            await this._session.SaveAsync(entity, cancellationToken);

        await this._session.FlushAsync();
    }

    public async Task UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, new()
    {
        if (entity == null)
            return;

        this._session.Clear();

        await this._session.UpdateAsync(entity, cancellationToken);
        await this._session.FlushAsync();
    }

    public async Task UpdateAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class, new()
    {
        var entitiesArray = entities.Where(r => r != null).ToArrayOrEmpty();

        if (!entitiesArray.Any())
            return;

        this._session.Clear();

        foreach (var entity in entitiesArray)
            await this._session.UpdateAsync(entity, cancellationToken);

        await this._session.FlushAsync();
    }

    public async Task DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, new()
    {
        if (entity == null)
            return;

        this._session.Clear();

        await this._session.DeleteAsync(entity, cancellationToken);

        await this._session.FlushAsync();
    }

    public async Task DeleteAsync<TEntity>(TEntity[] entities, CancellationToken cancellationToken = default) where TEntity : class, new()
    {
        var entitiesArray = entities.Where(r => r != null).ToArrayOrEmpty();

        if (!entitiesArray.Any())
            return;

        this._session.Clear();

        foreach (var entity in entitiesArray)
            await this._session.DeleteAsync(entity, cancellationToken);

        await this._session.FlushAsync();
    }

    public IQueryable<TEntity> Read<TEntity>(Specification<TEntity> specification = default,
                                            IEnumerable<OrderSpecification<TEntity>> orderSpecifications = default)
            where TEntity : class, new()
    {
        this._session.Clear();

        var queryable = this._session.Query<TEntity>();

        var expression = specification?.ToExpression();

        return (expression == null ? queryable : queryable.Where(expression)).ApplyOrderSpecifications(orderSpecifications);
    }

    #endregion
}