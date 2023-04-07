namespace CRUD.DAL
{
    #region << Using >>

    using System.Linq;
    using LinqSpecs;

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

        public IQueryable<TEntity> Get(Specification<TEntity> specification = null)
        {
            var dbSet = this._context.Set<TEntity>();

            return specification == null ?
                           dbSet.AsQueryable() :
                           dbSet.Where(specification).AsQueryable();
        }

        #endregion
    }
}