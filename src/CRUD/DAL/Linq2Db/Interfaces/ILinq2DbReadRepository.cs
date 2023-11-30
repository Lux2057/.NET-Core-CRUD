namespace CRUD.DAL.Linq2Db;

#region << Using >>

using CRUD.DAL.Abstractions;
using LinqSpecs;

#endregion

public interface ILinq2DbReadRepository : IReadRepository
{
    /// <summary>
    ///     Supported only by Linq2Db.
    ///     Returns a IQueryable response from a data storage.
    /// </summary>
    IQueryable<TEntity> Read<TEntity>(Specification<TEntity> specification = default,
                                      string tableName = default,
                                      IEnumerable<OrderSpecification<TEntity>> orderSpecifications = default)
            where TEntity : class, new();
}