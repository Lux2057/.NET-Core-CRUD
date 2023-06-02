namespace CRUD.DAL.NHibernate;

#region << Using >>

using System.Data;
using CRUD.DAL.Abstractions;
using global::NHibernate;

#endregion

/// <summary>
///     EntityFrameworkCore based implementation of the IUnitOfWork interface
/// </summary>
public class NhUnitOfWork : IUnitOfWork
{
    #region Properties

    public IRepository Repository { get; }

    public string OpenedTransactionId { get; private set; }

    public bool IsTransactionOpened { get; private set; }

    private readonly ISession _session;

    #endregion

    #region Constructors

    public NhUnitOfWork(ISession session, IRepository repository)
    {
        this._session = session;
        Repository = repository;
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