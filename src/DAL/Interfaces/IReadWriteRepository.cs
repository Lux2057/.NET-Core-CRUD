namespace CRUD.DAL
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    public interface IReadWriteRepository<TEntity> : IReadRepository<TEntity> where TEntity : class, new()
    {
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    }
}