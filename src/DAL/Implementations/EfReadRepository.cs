namespace CRUD.DAL
{
    #region << Using >>

    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using LinqSpecs;
    using Microsoft.EntityFrameworkCore;

    #endregion

    public class EfReadRepository<TEntity> : IReadRepository<TEntity> where TEntity : class, new()
    {
        #region Properties

        protected readonly IEfDbContext _context;

        #endregion

        #region Constructors

        public EfReadRepository(IEfDbContext context)
        {
            this._context = context;
        }

        #endregion

        #region Interface Implementations

        public IQueryable<TEntity> Get(Specification<TEntity> specification = default)
        {
            var dbSet = this._context.Set<TEntity>();

            return specification == null ?
                           dbSet.AsQueryable() :
                           dbSet.Where(specification).AsQueryable();
        }

        public IQueryable<TEntity> GetPaginated(Specification<TEntity> specification = default, int? page = default, int? pageSize = default)
        {
            var totalCount = Get(specification).Count();

            return Get(specification).GetPage(totalCount, page, pageSize);
        }

        public async Task<IQueryable<TEntity>> GetPaginatedAsync(Specification<TEntity> specification = default, int? page = default, int? pageSize = default, CancellationToken cancellationToken = default)
        {
            var totalCount = await Get(specification).CountAsync(cancellationToken);

            return Get(specification).GetPage(totalCount, page, pageSize);
        }

        #endregion
    }
}