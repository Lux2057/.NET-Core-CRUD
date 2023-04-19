namespace CRUD.DAL
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    public interface IReadWriteRepository<TEntity> : IReadRepository<TEntity> where TEntity : class, new()
    {
        /// <summary>
        ///     Adds an entity to a data storage
        /// </summary>
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Adds entities to a data storage
        /// </summary>
        Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Updates an entity in a data storage
        /// </summary>
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Updates entities in a data storage
        /// </summary>
        Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Deletes an entity from a data storage
        /// </summary>
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Deletes entities from a data storage
        /// </summary>
        Task DeleteAsync(TEntity[] entities, CancellationToken cancellationToken = default);
    }
}