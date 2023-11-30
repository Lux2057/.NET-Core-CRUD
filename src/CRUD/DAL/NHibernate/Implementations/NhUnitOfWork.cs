namespace CRUD.DAL.NHibernate;

#region << Using >>

using System.Data;
using CRUD.DAL.Abstractions;
using global::NHibernate;

#endregion

/// <summary>
///     NHibernate based implementation of the IUnitOfWork interface.
/// </summary>
public class NhUnitOfWork : IUnitOfWork
{
    #region Properties

    public string OpenedTransactionId { get; private set; }

    public bool IsTransactionOpened { get; private set; }

    private readonly ISession _session;

    #endregion

    #region Constructors

    public NhUnitOfWork(ISession session)
    {
        this._session = session;
    }

    #endregion

    #region Interface Implementations

    public void OpenTransaction(IsolationLevel isolationLevel)
    {
        if (this._session.GetCurrentTransaction() != null)
            return;

        this._session.BeginTransaction(isolationLevel);
        OpenedTransactionId = Guid.NewGuid().ToString();
        IsTransactionOpened = true;
    }

    public void CloseTransaction()
    {
        var currentTransaction = this._session.GetCurrentTransaction();

        if (currentTransaction == null)
            return;

        this._session.Flush();

        currentTransaction.Commit();
        currentTransaction.Dispose();

        OpenedTransactionId = string.Empty;
        IsTransactionOpened = false;
    }

    public void RollbackTransaction()
    {
        var currentTransaction = this._session.GetCurrentTransaction();

        if (currentTransaction == null)
            return;

        currentTransaction.Rollback();
        currentTransaction.Dispose();

        OpenedTransactionId = string.Empty;
        IsTransactionOpened = false;
    }

    #endregion
}