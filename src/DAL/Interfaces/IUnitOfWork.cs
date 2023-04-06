namespace CRUD.DAL
{
    #region << Using >>

    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    public interface IUnitOfWork
    {
        public IReadRepository<TEntity> ReadRepository<TEntity>() where TEntity : class, new();

        public IReadWriteRepository<TEntity> ReadWriteRepository<TEntity>() where TEntity : class, new();

        public Task BeginTransactionAsync(PermissionType permissionType, CancellationToken cancellationToken = default);

        public Task EndTransactionAsync(CancellationToken cancellationToken = default);

        public Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}