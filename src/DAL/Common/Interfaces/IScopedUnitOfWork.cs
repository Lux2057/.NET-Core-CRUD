namespace CRUD.DAL
{
    #region << Using >>

    using System.Data;

    #endregion

    /// <summary>
    ///     Unit of work pattern interface for transaction scope support
    /// </summary>
    public interface IScopedUnitOfWork
    {
        #region Properties

        public IRepository Repository { get; }

        #endregion

        /// <summary>
        ///     Starts a transaction scope
        /// </summary>
        /// <returns>If a transaction scope doesn't exist returns transaction id, otherwise returns empty string</returns>
        public string BeginTransactionScope(IsolationLevel isolationLevel);

        /// <summary>
        ///     Ends currently opened transaction scope
        /// </summary>
        public void EndTransactionScope();

        /// <summary>
        ///     Rolls back all changes in currently opened transaction scope
        /// </summary>
        public void RollbackCurrentTransactionScope();
    }
}