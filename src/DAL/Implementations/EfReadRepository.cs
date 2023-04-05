namespace CRUD.DAL
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using LinqSpecs;
    using Microsoft.EntityFrameworkCore;

    public class EfReadRepository<TEntity, TId> : IReadRepository<TEntity, TId> where TEntity : class, IId<TId>, new()
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

        public async Task<TEntity> GetByIdOrDefaultAsync(TId id, CancellationToken cancellationToken = default)
        {
            return await this._context.Set<TEntity>().SingleOrDefaultAsync(cancellationToken: cancellationToken, predicate: r => Equals(r.Id, id));
        }

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
    }}