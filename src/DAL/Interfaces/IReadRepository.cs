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
    }
}