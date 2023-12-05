namespace CRUD.DAL.Linq2Db;

#region << Using >>

using CRUD.DAL.Abstractions;
using LinqToDB;
using LinqToDB.Data;

#endregion

public interface ILinq2DbRepository : IRepository, ILinq2DbReadRepository
{
    /// <summary>
    ///     Supported only by Linq2Db.
    ///     Adds an entity to a data storage.
    /// </summary>
    Task CreateAsync<TEntity>(TEntity entity,
                              string tableName,
                              CancellationToken cancellationToken = default) where TEntity : class, new();

    /// <summary>
    ///     Supported only by Linq2Db.
    ///     Adds entities to a data storage.
    /// </summary>
    Task CreateAsync<TEntity>(IEnumerable<TEntity> entities,
                              BulkCopyOptions options,
                              CancellationToken cancellationToken = default) where TEntity : class, new();

    /// <summary>
    ///     Supported only by Linq2Db.
    ///     Updates an entity in a data storage.
    /// </summary>
    Task UpdateAsync<TEntity>(TEntity entity,
                              string tableName,
                              CancellationToken cancellationToken = default) where TEntity : class, new();

    /// <summary>
    ///     Supported only by Linq2Db.
    ///     Updates entities in a data storage.
    /// </summary>
    Task UpdateAsync<TEntity>(IEnumerable<TEntity> entities,
                              string tableName,
                              CancellationToken cancellationToken = default) where TEntity : class, new();

    /// <summary>
    ///     Supported only by Linq2Db.
    ///     Deletes an entity from a data storage.
    /// </summary>
    Task DeleteAsync<TEntity>(TEntity entity,
                              string tableName,
                              CancellationToken cancellationToken = default) where TEntity : class, new();

    /// <summary>
    ///     Supported only by Linq2Db.
    ///     Deletes entities from a data storage.
    /// </summary>
    Task DeleteAsync<TEntity>(IEnumerable<TEntity> entities,
                              string tableName,
                              CancellationToken cancellationToken = default) where TEntity : class, new();

    /// <summary>
    ///     Supported only by Linq2Db.
    ///     Returns Linq2Db ITable.
    /// </summary>
    ITable<TEntity> GetTable<TEntity>(string tableName) where TEntity : class, new();
}