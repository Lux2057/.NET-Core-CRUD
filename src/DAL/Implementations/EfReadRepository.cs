namespace CRUD.DAL
{
    #region << Using >>

    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using LinqSpecs;
    using Microsoft.EntityFrameworkCore;

    #endregion

    #endregion

    /// <summary>
    ///     EntityFrameworkCore based implementation of the IReadRepository interface
    /// </summary>
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

        public IQueryable<TEntity> Get(Specification<TEntity> specification = default, IEnumerable<OrderSpecification<TEntity>> orderSpecifications = default)
        {
            var dbSet = this._context.Set<TEntity>();

            return (specification == null ? dbSet : dbSet.Where(specification)).ApplyOrderSpecifications(orderSpecifications).AsNoTracking();
        }

        #endregion
    }
}