namespace CRUD.DAL.LINQ2DB;

#region << Using >>

using CRUD.DAL.Abstractions;
using LinqToDB;

#endregion

public interface ILinq2DbRepository : IRepository, ILinq2DbReadRepository
{
    /// <summary>
    ///     Supported only by LINQ2DB.
    ///     Adds an entity to a data storage.
    /// </summary>
    Task CreateAsync<TEntity>(TEntity entity,
                              string tableName,
                              CancellationToken cancellationToken = default) where TEntity : class, new();

    /// <summary>
    ///     Supported only by LINQ2DB.
    ///     Adds entities to a data storage.
    /// </summary>
    Task CreateAsync<TEntity>(IEnumerable<TEntity> entities,
                              string tableName,
                              CancellationToken cancellationToken = default) where TEntity : class, new();

    /// <summary>
    ///     Supported only by LINQ2DB.
    ///     Updates an entity in a data storage.
    /// </summary>
    Task UpdateAsync<TEntity>(TEntity entity,
                              string tableName,
                              CancellationToken cancellationToken = default) where TEntity : class, new();

    /// <summary>
    ///     Supported only by LINQ2DB.
    ///     Updates entities in a data storage.
    /// </summary>
    Task UpdateAsync<TEntity>(IEnumerable<TEntity> entities,
                              string tableName,
                              CancellationToken cancellationToken = default) where TEntity : class, new();

    /// <summary>
    ///     Supported only by LINQ2DB.
    ///     Deletes an entity from a data storage.
    /// </summary>
    Task DeleteAsync<TEntity>(TEntity entity,
                              string tableName,
                              CancellationToken cancellationToken = default) where TEntity : class, new();

    /// <summary>
    ///     Supported only by LINQ2DB.
    ///     Deletes entities from a data storage.
    /// </summary>
    Task DeleteAsync<TEntity>(IEnumerable<TEntity> entities,
                              string tableName,
                              CancellationToken cancellationToken = default) where TEntity : class, new();

    /// <summary>
    ///     Supported only by LINQ2DB.
    ///     Returns LINQ2DB ITable.
    /// </summary>
    ITable<TEntity> GetTable<TEntity>(string tableName) where TEntity : class, new();
}