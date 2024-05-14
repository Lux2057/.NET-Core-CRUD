namespace CRUD.DAL.Abstractions;

#region << Using >>

#endregion

public interface IRepository : IReadRepository
{
    /// <summary>
    ///     Adds an entity to a data storage.
    /// </summary>
    Task CreateAsync<TEntity>(TEntity entity,
                              CancellationToken cancellationToken = default)
            where TEntity : class, new();

    /// <summary>
    ///     Adds entities to a data storage.
    /// </summary>
    Task CreateAsync<TEntity>(IEnumerable<TEntity> entities,
                              CancellationToken cancellationToken = default)
            where TEntity : class, new();

    /// <summary>
    ///     Updates an entity in a data storage.
    /// </summary>
    Task UpdateAsync<TEntity>(TEntity entity,
                              CancellationToken cancellationToken = default)
            where TEntity : class, new();

    /// <summary>
    ///     Updates entities in a data storage.
    /// </summary>
    Task UpdateAsync<TEntity>(IEnumerable<TEntity> entities,
                              CancellationToken cancellationToken = default)
            where TEntity : class, new();

    /// <summary>
    ///     Deletes an entity from a data storage.
    /// </summary>
    Task DeleteAsync<TEntity>(TEntity entity,
                              CancellationToken cancellationToken = default)
            where TEntity : class, new();

    /// <summary>
    ///     Deletes entities from a data storage.
    /// </summary>
    Task DeleteAsync<TEntity>(IEnumerable<TEntity> entities,
                              CancellationToken cancellationToken = default)
            where TEntity : class, new();
}