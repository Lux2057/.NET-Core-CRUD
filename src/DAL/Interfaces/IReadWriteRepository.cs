namespace CRUD.DAL
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    public interface IReadWriteRepository<TEntity, in TId> : IReadRepository<TEntity, TId> where TEntity : IId<TId>, new()
    {
        Task AddOrUpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task AddOrUpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task DeleteAsync(TId id, CancellationToken cancellationToken = default);

        Task DeleteAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default);
    }
}