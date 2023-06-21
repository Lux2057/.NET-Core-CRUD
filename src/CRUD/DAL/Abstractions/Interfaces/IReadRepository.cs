namespace CRUD.DAL.Abstractions
{
    #region << Using >>

    using LinqSpecs;

    #endregion

    public interface IReadRepository
    {
        /// <summary>
        ///     Returns a IQueryable response from a data storage
        /// </summary>
        IQueryable<TEntity> Read<TEntity>(Specification<TEntity> specification = default,
                                         IEnumerable<OrderSpecification<TEntity>> orderSpecifications = default)
                where TEntity : class, new();
    }
}