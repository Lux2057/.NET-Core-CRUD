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
        IQueryable<TEntity> Get(Specification<TEntity> specification = default);

        IQueryable<TEntity> GetPaginated(Specification<TEntity> specification = default, int? page = default, int? pageSize = default);

        Task<IQueryable<TEntity>> GetPaginatedAsync(Specification<TEntity> specification = default, int? page = default, int? pageSize = default, CancellationToken cancellationToken = default);
    }
}