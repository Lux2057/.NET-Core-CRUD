namespace CRUD.DAL;

#region << Using >>

using System;
using System.Data;
using NHibernate;

#endregion

/// <summary>
///     EntityFrameworkCore based implementation of the IUnitOfWork interface
/// </summary>
public class NHibernateScopedUnitOfWork : IScopedUnitOfWork
{
    #region Properties

    public IRepository Repository { get; }

    public string OpenedScopeId { get; private set; }

    public bool IsOpened { get; private set; }

    private readonly ISession _session;

    #endregion

    #region Constructors

    public NHibernateScopedUnitOfWork(ISession session, IRepository repository)
    {
        this._session = session;
        Repository = repository;
    }

    #endregion

    #region Interface Implementations

    public void OpenTransactionScope(IsolationLevel isolationLevel)
    {
        if (this._session.GetCurrentTransaction() != null)
            return;

        this._session.BeginTransaction(isolationLevel);
        OpenedScopeId = Guid.NewGuid().ToString();
        IsOpened = true;
    }

    public void CloseTransactionScope()
    {
        if (this._session.GetCurrentTransaction() == null)
            return;

        this._session.GetCurrentTransaction().Commit();
        OpenedScopeId = string.Empty;
        IsOpened = false;
    }

    public void RollbackChanges()
    {
        if (this._session.GetCurrentTransaction() == null)
            return;

        this._session.GetCurrentTransaction().Rollback();
    }

    #endregion
}