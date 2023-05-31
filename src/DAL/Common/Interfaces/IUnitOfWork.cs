namespace CRUD.DAL
{
    #region << Using >>

    using System;
    using System.Data;
    using System.Threading.Tasks;

    #endregion

    /// <summary>
    ///     Unit of work pattern interface for transaction scope support
    /// </summary>
    public interface IUnitOfWork
    {
        #region Properties

        public IRepository Repository { get; }

        #endregion

        /// <summary>
        ///     Starts a transaction scope
        /// </summary>
        public Task<string> BeginTransactionScopeAsync(IsolationLevel isolationLevel);

        /// <summary>
        ///     Ends a transaction scope by transaction id
        /// </summary>
        public void EndTransactionScope(string transactionId);

        /// <summary>
        ///     Ends a transaction scope by transaction id
        /// </summary>
        public Task EndTransactionScopeAsync(string transactionId);

        /// <summary>
        ///     Rolls back all changes in currently opened transaction scope
        /// </summary>
        public Task RollbackCurrentTransactionScopeAsync();

        /// <summary>
        ///     Rolls back all changes in currently opened transaction scope
        /// </summary>
        public void RollbackCurrentTransactionScope();
    }
}