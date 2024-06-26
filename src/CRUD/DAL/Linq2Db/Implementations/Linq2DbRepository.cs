﻿namespace CRUD.DAL.Linq2Db;

#region << Using >>

using CRUD.DAL.Abstractions;
using LinqSpecs;
using LinqToDB;
using LinqToDB.Data;

#endregion

public class Linq2DbRepository<TDataConnection> : ILinq2DbRepository
        where TDataConnection : DataConnection
{
    #region Properties

    readonly TDataConnection _connection;

    #endregion

    #region Constructors

    public Linq2DbRepository(TDataConnection connection)
    {
        this._connection = connection;
    }

    #endregion

    #region Interface Implementations

    public IQueryable<TEntity> Read<TEntity>(Specification<TEntity> specification = default,
                                             IEnumerable<OrderSpecification<TEntity>> orderSpecifications = default)
            where TEntity : class, new()
    {
        var table = this._connection.GetTable<TEntity>();

        return (specification == null ? table : table.Where(specification)).ApplyOrderSpecifications(orderSpecifications);
    }

    public IQueryable<TEntity> Read<TEntity>(Specification<TEntity> specification = default,
                                             string tableName = default,
                                             IEnumerable<OrderSpecification<TEntity>> orderSpecifications = default)
            where TEntity : class, new()
    {
        var table = string.IsNullOrWhiteSpace(tableName) ?
                            this._connection.GetTable<TEntity>() :
                            this._connection.GetTable<TEntity>().TableName(tableName);

        return (specification == null ? table : table.Where(specification)).ApplyOrderSpecifications(orderSpecifications);
    }

    public async Task CreateAsync<TEntity>(TEntity entity,
                                           CancellationToken cancellationToken = default)
            where TEntity : class, new()
    {
        if (entity == null)
            return;

        await this._connection.InsertAsync(obj: entity,
                                           token: cancellationToken);
    }

    public async Task CreateAsync<TEntity>(IEnumerable<TEntity> entities,
                                           CancellationToken cancellationToken = default)
            where TEntity : class, new()
    {
        if (entities == null)
            return;

        var entitiesArray = entities.Where(r => r != null).ToArray();

        if (entitiesArray.Length == 0)
            return;

        var result = await this._connection.BulkCopyAsync(options: new BulkCopyOptions { BulkCopyType = BulkCopyType.MultipleRows },
                                                          source: entitiesArray,
                                                          cancellationToken: cancellationToken);

        if (entitiesArray.Length != result.RowsCopied)
            throw new InvalidOperationException($"Copied rows count ({result.RowsCopied}) is not equal to expected ({entitiesArray.Length})");
    }

    public async Task UpdateAsync<TEntity>(TEntity entity,
                                           CancellationToken cancellationToken = default)
            where TEntity : class, new()
    {
        if (entity == null)
            return;

        await this._connection.UpdateAsync(obj: entity,
                                           token: cancellationToken);
    }

    public async Task UpdateAsync<TEntity>(IEnumerable<TEntity> entities,
                                           CancellationToken cancellationToken = default)
            where TEntity : class, new()
    {
        if (entities == null)
            return;

        var entitiesArray = entities.Where(r => r != null).ToArray();

        if (entitiesArray.Length == 0)
            return;

        foreach (var entity in entitiesArray)
            await this._connection.UpdateAsync(obj: entity,
                                               token: cancellationToken);
    }

    public async Task DeleteAsync<TEntity>(TEntity entity,
                                           CancellationToken cancellationToken = default)
            where TEntity : class, new()
    {
        if (entity == null)
            return;

        await this._connection.DeleteAsync(obj: entity,
                                           token: cancellationToken);
    }

    public async Task DeleteAsync<TEntity>(IEnumerable<TEntity> entities,
                                           CancellationToken cancellationToken = default)
            where TEntity : class, new()
    {
        if (entities == null)
            return;

        var entitiesArray = entities.Where(r => r != null).ToArray();

        if (entitiesArray.Length == 0)
            return;

        foreach (var entity in entitiesArray)
            await this._connection.DeleteAsync(obj: entity,
                                               token: cancellationToken);
    }

    public async Task CreateAsync<TEntity>(TEntity entity,
                                           string tableName,
                                           CancellationToken cancellationToken = default)
            where TEntity : class, new()
    {
        if (entity == null)
            return;

        await this._connection.InsertAsync(obj: entity,
                                           tableName: tableName,
                                           token: cancellationToken);
    }

    public async Task CreateAsync<TEntity>(IEnumerable<TEntity> entities,
                                           BulkCopyOptions options,
                                           CancellationToken cancellationToken = default)
            where TEntity : class, new()
    {
        if (entities == null)
            return;

        var entitiesArray = entities.Where(r => r != null).ToArray();

        if (entitiesArray.Length == 0)
            return;

        var result = await this._connection.BulkCopyAsync(options: options,
                                                          source: entitiesArray,
                                                          cancellationToken: cancellationToken);

        if (entitiesArray.Length != result.RowsCopied)
            throw new InvalidOperationException($"Copied rows count ({result.RowsCopied}) is not equal to expected ({entitiesArray.Length})");
    }

    public async Task UpdateAsync<TEntity>(TEntity entity,
                                           string tableName,
                                           CancellationToken cancellationToken = default)
            where TEntity : class, new()
    {
        if (entity == null)
            return;

        await this._connection.UpdateAsync(obj: entity,
                                           tableName: tableName,
                                           token: cancellationToken);
    }

    public async Task UpdateAsync<TEntity>(IEnumerable<TEntity> entities,
                                           string tableName,
                                           CancellationToken cancellationToken = default)
            where TEntity : class, new()
    {
        if (entities == null)
            return;

        var entitiesArray = entities.Where(r => r != null).ToArray();

        if (entitiesArray.Length == 0)
            return;

        foreach (var entity in entitiesArray)
            await this._connection.UpdateAsync(obj: entity,
                                               tableName: tableName,
                                               token: cancellationToken);
    }

    public async Task DeleteAsync<TEntity>(TEntity entity,
                                           string tableName,
                                           CancellationToken cancellationToken = default)
            where TEntity : class, new()
    {
        if (entity == null)
            return;

        await this._connection.DeleteAsync(obj: entity,
                                           tableName: tableName,
                                           token: cancellationToken);
    }

    public async Task DeleteAsync<TEntity>(IEnumerable<TEntity> entities,
                                           string tableName,
                                           CancellationToken cancellationToken = default)
            where TEntity : class, new()
    {
        if (entities == null)
            return;

        var entitiesArray = entities.Where(r => r != null).ToArray();

        if (entitiesArray.Length == 0)
            return;

        foreach (var entity in entitiesArray)
            await this._connection.DeleteAsync(obj: entity,
                                               tableName: tableName,
                                               token: cancellationToken);
    }

    public ITable<TEntity> GetTable<TEntity>(string tableName = default)
            where TEntity : class, new()
    {
        return string.IsNullOrWhiteSpace(tableName) ?
                       this._connection.GetTable<TEntity>() :
                       this._connection.GetTable<TEntity>().TableName(tableName);
    }

    #endregion
}