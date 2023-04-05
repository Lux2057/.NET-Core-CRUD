namespace CRUD.DAL
{
    #region << Using >>

    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    public interface IUnitOfWork
    {
        public IReadRepository<TEntity, TId> ReadRepository<TEntity, TId>() where TEntity : IId<TId>, new();

        public IReadWriteRepository<TEntity, TId> ReadWriteRepository<TEntity, TId>() where TEntity : IId<TId>, new();

        public Task BeginTransactionAsync(PermissionType permissionType, CancellationToken cancellationToken = default);

        public Task EndTransactionAsync(CancellationToken cancellationToken = default);

        public Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}