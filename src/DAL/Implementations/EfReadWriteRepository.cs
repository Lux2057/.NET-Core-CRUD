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

    public class EfReadWriteRepository<TEntity> : EfReadRepository<TEntity>, IReadWriteRepository<TEntity> where TEntity : EntityBase, new()
    {
        #region Constructors

        public EfReadWriteRepository(IEfDbContext context) : base(context) { }

        #endregion

        #region Interface Implementations

        public async Task AddOrUpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var dbSet = this._context.Set<TEntity>();

            if (await dbSet.AnyAsync(predicate: r => Equals(r.Id, entity.Id), cancellationToken: cancellationToken))
                dbSet.Update(entity);
            else
                dbSet.Add(entity);

            await this._context.SaveChangesAsync(cancellationToken);
        }

        public async Task AddOrUpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            var entitiesArray = entities.ToArrayOrEmpty();

            if (!entitiesArray.Any())
                return;

            var dbSet = this._context.Set<TEntity>();

            var entitiesIds = entitiesArray.GetIds();
            var existingEntitiesIds = await dbSet.Where(r => entitiesIds.Contains(r.Id))
                                                 .Select(r => r.Id).ToArrayAsync(cancellationToken);

            dbSet.UpdateRange(entitiesArray.Where(r => existingEntitiesIds.Contains(r.Id)));
            await dbSet.AddRangeAsync(entitiesArray.Where(r => !existingEntitiesIds.Contains(r.Id)), cancellationToken);

            await this._context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            this._context.Set<TEntity>().Remove(entity);

            await this._context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            var entitiesArray = entities.ToArrayOrEmpty();
            if (!entitiesArray.Any())
                return;

            this._context.Set<TEntity>().RemoveRange(entitiesArray);

            await this._context.SaveChangesAsync(cancellationToken);
        }

        #endregion

        public async Task DeleteAsync(object id, CancellationToken cancellationToken = default)
        {
            var entity = await GetByIdOrDefaultAsync(id: id, cancellationToken: cancellationToken);

            this._context.Set<TEntity>().Remove(entity);

            await this._context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(IEnumerable<object> ids, CancellationToken cancellationToken = default)
        {
            var idsArray = ids.ToArrayOrEmpty();

            if (!idsArray.Any())
                return;

            var dbSet = this._context.Set<TEntity>();

            var entities = await dbSet.Where(r => idsArray.Contains(r.Id)).ToArrayAsync(cancellationToken);

            if (!entities.Any())
                return;

            dbSet.RemoveRange(entities);

            await this._context.SaveChangesAsync(cancellationToken);
        }
    }
}