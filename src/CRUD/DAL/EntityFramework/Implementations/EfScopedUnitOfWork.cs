namespace CRUD.DAL.EntityFramework
{
    #region << Using >>

    using System.Data;
    using CRUD.DAL.Abstractions;
    using Microsoft.EntityFrameworkCore;

    #endregion

    /// <summary>
    ///     EntityFrameworkCore based implementation of the IUnitOfWork interface
    /// </summary>
    public class EfScopedUnitOfWork : IScopedUnitOfWork
    {
        #region Properties

        public IRepository Repository { get; }

        public string OpenedScopeId { get; private set; }

        public bool IsOpened { get; private set; }

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

        public void OpenScope(IsolationLevel isolationLevel)
        {
            if (this._dbContext.Database.CurrentTransaction != null)
                return;

            var transaction = this._dbContext.Database.BeginTransaction(isolationLevel);
            OpenedScopeId = transaction.TransactionId.ToString();
            IsOpened = true;
        }

        public void CloseScope()
        {
            if (this._dbContext.Database.CurrentTransaction == null)
                return;

            this._dbContext.Database.CurrentTransaction.Commit();
            OpenedScopeId = string.Empty;
            IsOpened = false;
        }

        public void RollbackAndCloseScope()
        {
            if (this._dbContext.Database.CurrentTransaction == null)
                return;

            this._dbContext.Database.CurrentTransaction.Rollback();
        }

        #endregion
    }
}