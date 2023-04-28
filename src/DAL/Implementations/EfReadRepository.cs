﻿namespace CRUD.DAL
{
    #region << Using >>

    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CRUD.Extensions;
    using LinqSpecs;
    using Microsoft.EntityFrameworkCore;

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

        public IQueryable<TEntity> Get(Specification<TEntity> specification = default)
        {
            var dbSet = this._context.Set<TEntity>();

            return (specification == null ?
                            dbSet.AsQueryable() :
                            dbSet.Where(specification).AsQueryable()).AsNoTracking();
        }

        public async Task<IQueryable<TEntity>> GetPageAsync(Specification<TEntity> specification = default, int? page = default, int? pageSize = default, CancellationToken cancellationToken = default)
        {
            var totalCount = await Get(specification).AsNoTracking().CountAsync(cancellationToken);

            return Get(specification).ToPage(totalCount, page, pageSize);
        }

        #endregion
    }
}