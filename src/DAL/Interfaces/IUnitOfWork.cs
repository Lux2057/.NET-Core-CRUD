namespace CRUD.DAL
{
    #region << Using >>

    using System.Data;
    using System.Threading.Tasks;

    #endregion

    public interface IUnitOfWork
    {
        public IReadRepository<TEntity> ReadRepository<TEntity>() where TEntity : class, new();

        public IReadWriteRepository<TEntity> ReadWriteRepository<TEntity>() where TEntity : class, new();

        public Task<string> BeginTransactionScopeAsync(IsolationLevel isolationLevel);

        public Task EndTransactionScopeAsync(string transactionId);

        public Task RollbackCurrentTransactionScopeAsync();
    }
}