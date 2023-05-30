namespace CRUD.DAL
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using LinqSpecs;

    #endregion

    public interface IReadRepository<TEntity> where TEntity : class, new()
    {
        /// <summary>
        ///     Returns a IQueryable response from a data storage
        /// </summary>
        IQueryable<TEntity> Get(Specification<TEntity> specification = default, IEnumerable<OrderSpecification<TEntity>> orderSpecifications = default);
    }
}