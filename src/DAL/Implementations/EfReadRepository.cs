namespace CRUD.DAL
{
    #region << Using >>

    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using LinqSpecs;
    using Microsoft.EntityFrameworkCore;

    #endregion

    public class EfReadRepository<TEntity> : IReadRepository<TEntity> where TEntity : EntityBase, new()
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

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicateExpression = null)
        {
            var predicate = predicateExpression?.Compile();

            var dbSet = this._context.Set<TEntity>();

            return predicate == null ?
                           dbSet.AsQueryable() :
                           dbSet.Where(predicate).AsQueryable();
        }

        public IQueryable<TEntity> Get(Specification<TEntity> specification = null)
        {
            var dbSet = this._context.Set<TEntity>();

            return specification == null ?
                           dbSet.AsQueryable() :
                           dbSet.Where(specification).AsQueryable();
        }

        #endregion

        public async Task<TEntity> GetByIdOrDefaultAsync(object id, CancellationToken cancellationToken = default)
        {
            return await this._context.Set<TEntity>().SingleOrDefaultAsync(cancellationToken: cancellationToken, predicate: r => Equals(r.Id, id));
        }
    }
}