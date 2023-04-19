namespace CRUD.DAL
{
    #region << Using >>

    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using LinqSpecs;

    #endregion

    public interface IReadRepository<TEntity> where TEntity : class, new()
    {
        /// <summary>
        ///     Returns a IQueryable response from a data storage
        /// </summary>
        IQueryable<TEntity> Get(Specification<TEntity> specification = default);

        /// <summary>
        ///     Returns a page of IQueryable response from a data storage
        /// </summary>
        Task<IQueryable<TEntity>> GetPageAsync(Specification<TEntity> specification = default, int? page = default, int? pageSize = default, CancellationToken cancellationToken = default);
    }
}