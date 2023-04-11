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
        IQueryable<TEntity> Get(Specification<TEntity> specification);

        IQueryable<TEntity> GetPaginated(Specification<TEntity> specification, int? page, int? pageSize);

        Task<IQueryable<TEntity>> GetPaginatedAsync(Specification<TEntity> specification, int? page, int? pageSize, CancellationToken cancellationToken = default);
    }
}