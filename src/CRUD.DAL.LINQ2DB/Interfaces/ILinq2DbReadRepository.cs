namespace CRUD.DAL.LINQ2DB;

#region << Using >>

using CRUD.DAL.Abstractions;
using LinqSpecs;

#endregion

public interface ILinq2DbReadRepository : IReadRepository
{
    /// <summary>
    ///     Supported only by LINQ2DB
    ///     Returns a IQueryable response from a data storage
    /// </summary>
    IQueryable<TEntity> Read<TEntity>(Specification<TEntity> specification = default,
                                      string tableName = default,
                                      IEnumerable<OrderSpecification<TEntity>> orderSpecifications = default)
            where TEntity : class, new();
}