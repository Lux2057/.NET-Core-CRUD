namespace CRUD.DAL
{
    #region << Using >>

    using System.Data;
    using Microsoft.EntityFrameworkCore;

    #endregion

    /// <summary>
    ///     EntityFrameworkCore based implementation of the IUnitOfWork interface
    /// </summary>
    public class EfScopedUnitOfWork : IScopedUnitOfWork
    {
        #region Properties

        public IRepository Repository { get; }

        private readonly IEfDbContext _dbContext;

        #endregion

        #region Constructors

        public EfScopedUnitOfWork(IRepository repository, IEfDbContext efDbContext)
        {
            this._dbContext = efDbContext;
            Repository = repository;
        }

        #endregion

        #region Interface Implementations

        public string BeginTransactionScope(IsolationLevel isolationLevel)
        {
            if (this._dbContext.Database.CurrentTransaction != null)
                return string.Empty;

            this._dbContext.Database.BeginTransaction(isolationLevel);

            return this._dbContext.Database.CurrentTransaction!.TransactionId.ToString();
        }

        public void EndTransactionScope()
        {
            if (this._dbContext.Database.CurrentTransaction == null)
                return;

            this._dbContext.Database.CurrentTransaction.Commit();
        }

        public void RollbackCurrentTransactionScope()
        {
            if (this._dbContext.Database.CurrentTransaction == null)
                return;

            this._dbContext.Database.CurrentTransaction.Rollback();
        }

        #endregion
    }
}