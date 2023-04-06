namespace CRUD.DAL
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    public interface IReadWriteRepository<TEntity> : IReadRepository<TEntity> where TEntity : class, new()
    {
        Task AddOrUpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task AddOrUpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    }
}