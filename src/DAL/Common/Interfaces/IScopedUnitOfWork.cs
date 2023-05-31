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

        public string OpenedScopeId { get; }

        public bool IsOpened { get; }

        #endregion

        /// <summary>
        ///     Opens a transaction scope
        /// </summary>
        public void OpenScope(IsolationLevel isolationLevel);

        /// <summary>
        ///     Closes currently opened transaction scope
        /// </summary>
        public void CloseScope();

        /// <summary>
        ///     Rolls back all changes in currently opened transaction scope
        /// </summary>
        public void RollbackAndCloseScope();
    }
}