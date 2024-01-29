namespace CRUD.DAL.Abstractions;

#region << Using >>

using System.Data;

#endregion

/// <summary>
///     Unit of work pattern interface for transaction scope support.
/// </summary>
public interface IUnitOfWork
{
    #region Properties

    public string OpenedTransactionId { get; }

    public bool IsTransactionOpened { get; }

    #endregion

    /// <summary>
    ///     Opens a transaction.
    /// </summary>
    public void OpenTransaction(IsolationLevel isolationLevel);

    /// <summary>
    ///     Closes currently opened transaction.
    /// </summary>
    public void CloseTransaction();

    /// <summary>
    ///     Rolls back all changes in currently opened transaction.
    /// </summary>
    public void RollbackTransaction();
}