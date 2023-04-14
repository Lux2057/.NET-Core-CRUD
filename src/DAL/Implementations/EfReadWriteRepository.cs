namespace CRUD.DAL
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CRUD.Extensions;
    using Microsoft.EntityFrameworkCore;

    #endregion

    public class EfReadWriteRepository<TEntity> : EfReadRepository<TEntity>, IReadWriteRepository<TEntity> where TEntity : class, new()
    {
        #region Constructors

        public EfReadWriteRepository(IEfDbContext context) : base(context) { }

        #endregion

        #region Interface Implementations

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
                return;

            var dbSet = this._context.Set<TEntity>();

            dbSet.Add(entity);

            await this._context.SaveChangesAsync(cancellationToken);
        }

        public async Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            var entitiesArray = entities.Where(r => r != null).ToArrayOrEmpty();

            if (!entitiesArray.Any())
                return;

            var dbSet = this._context.Set<TEntity>();

            await dbSet.AddRangeAsync(entitiesArray, cancellationToken);

            await this._context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
                return;

            var dbSet = this._context.Set<TEntity>();

            dbSet.Update(entity);

            await this._context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            var entitiesArray = entities.Where(r => r != null).ToArrayOrEmpty();

            if (!entitiesArray.Any())
                return;

            var dbSet = this._context.Set<TEntity>();

            dbSet.UpdateRange(entitiesArray);

            await this._context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
                return;

            this._context.Set<TEntity>().Remove(entity);

            await this._context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(TEntity[] entities, CancellationToken cancellationToken = default)
        {
            var entitiesArray = entities.Where(r => r != null).ToArrayOrEmpty();

            if (!entitiesArray.Any())
                return;

            this._context.Set<TEntity>().RemoveRange(entitiesArray);

            await this._context.SaveChangesAsync(cancellationToken);
        }

        #endregion
    }
}