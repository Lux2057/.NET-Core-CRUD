namespace CRUD.DAL
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    public class EfReadWriteRepository<TEntity> : EfReadRepository<TEntity>, IReadWriteRepository<TEntity> where TEntity : class, new()
    {
        #region Constants

        private const string EntityCanTBeNull = "Entity can't be null!";

        private const string EntitiesCanTBeEmptyOrNull = "Entities can't be empty or null!";

        #endregion

        #region Constructors

        public EfReadWriteRepository(IEfDbContext context) : base(context) { }

        #endregion

        #region Interface Implementations

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
                throw new ArgumentException(EntityCanTBeNull);

            var dbSet = this._context.Set<TEntity>();

            dbSet.Add(entity);

            await this._context.SaveChangesAsync(cancellationToken);
        }

        public async Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            var entitiesArray = entities.ToArrayOrEmpty();

            if (!entitiesArray.Any() || entitiesArray.Any(r => r == null))
                throw new ArgumentException(EntitiesCanTBeEmptyOrNull);

            var dbSet = this._context.Set<TEntity>();

            await dbSet.AddRangeAsync(entitiesArray, cancellationToken);

            await this._context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
                throw new ArgumentException(EntityCanTBeNull);

            var dbSet = this._context.Set<TEntity>();

            dbSet.Update(entity);

            await this._context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            var entitiesArray = entities.ToArrayOrEmpty();

            if (!entitiesArray.Any() || entitiesArray.Any(r => r == null))
                throw new ArgumentException(EntitiesCanTBeEmptyOrNull);

            var dbSet = this._context.Set<TEntity>();

            dbSet.UpdateRange(entitiesArray);

            await this._context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
                throw new ArgumentException(EntityCanTBeNull);

            this._context.Set<TEntity>().Remove(entity);

            await this._context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            var entitiesArray = entities.ToArrayOrEmpty();

            if (!entitiesArray.Any() || entitiesArray.Any(r => r == null))
                throw new ArgumentException(EntitiesCanTBeEmptyOrNull);

            this._context.Set<TEntity>().RemoveRange(entitiesArray);

            await this._context.SaveChangesAsync(cancellationToken);
        }

        #endregion
    }
}