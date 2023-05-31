namespace CRUD.DAL
{
    #region << Using >>

    using System.Data;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    #endregion

    /// <summary>
    ///     EntityFrameworkCore based implementation of the IUnitOfWork interface
    /// </summary>
    public class EfUnitOfWork : IUnitOfWork
    {
        #region Properties

        public IRepository Repository { get; }

        private readonly IEfDbContext _dbContext;

        #endregion

        #region Constructors

        public EfUnitOfWork(IRepository repository, IEfDbContext efDbContext)
        {
            this._dbContext = efDbContext;
            Repository = repository;
        }

        #endregion

        #region Interface Implementations

        /// <summary>
        ///     Starts a transaction scope
        /// </summary>
        /// <returns>If a transaction scope doesn't exist returns transaction id, otherwise returns empty string</returns>
        public async Task<string> BeginTransactionScopeAsync(IsolationLevel isolationLevel)
        {
            if (this._dbContext.Database.CurrentTransaction != null)
                return string.Empty;

            await this._dbContext.Database.BeginTransactionAsync(isolationLevel);

            return this._dbContext.Database.CurrentTransaction!.TransactionId.ToString();
        }

        public void EndTransactionScope(string transactionId)
        {
            if (this._dbContext.Database.CurrentTransaction == null ||
                this._dbContext.Database.CurrentTransaction.TransactionId.ToString() != transactionId)
                return;

            this._dbContext.Database.CurrentTransaction.Commit();
        }

        public async Task EndTransactionScopeAsync(string transactionId)
        {
            if (this._dbContext.Database.CurrentTransaction == null ||
                this._dbContext.Database.CurrentTransaction.TransactionId.ToString() != transactionId)
                return;

            await this._dbContext.Database.CurrentTransaction.CommitAsync();
        }

        public async Task RollbackCurrentTransactionScopeAsync()
        {
            if (this._dbContext.Database.CurrentTransaction == null)
                return;

            await this._dbContext.Database.CurrentTransaction.RollbackAsync();
        }

        public void RollbackCurrentTransactionScope()
        {
            if (this._dbContext.Database.CurrentTransaction == null)
                return;

            this._dbContext.Database.CurrentTransaction.Rollback();
        }

        #endregion

        public string BeginTransactionScope(IsolationLevel isolationLevel)
        {
            if (this._dbContext.Database.CurrentTransaction != null)
                return string.Empty;

            this._dbContext.Database.BeginTransaction(isolationLevel);

            return this._dbContext.Database.CurrentTransaction!.TransactionId.ToString();
        }
    }
}