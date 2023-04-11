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

        public Task BeginTransactionAsync(PermissionType permissionType);

        public Task EndTransactionAsync();

        public Task RollbackTransactionAsync();
    }
}