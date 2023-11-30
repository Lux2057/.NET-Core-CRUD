﻿namespace CRUD.DAL.Linq2Db;

#region << Using >>

using System.Data;
using CRUD.DAL.Abstractions;
using LinqToDB.Data;

#endregion

/// <summary>
///     Linq2Db based implementation of the IUnitOfWork interface.
/// </summary>
public class Linq2DbUnitOfWork : IUnitOfWork
{
    #region Properties

    public string OpenedTransactionId { get; private set; }

    public bool IsTransactionOpened { get; private set; }

    readonly DataConnection _connection;

    #endregion

    #region Constructors

    public Linq2DbUnitOfWork(DataConnection connection)
    {
        this._connection = connection;
    }

    #endregion

    #region Interface Implementations

    public void OpenTransaction(IsolationLevel isolationLevel)
    {
        if (this._connection.Transaction != null)
            return;

        this._connection.BeginTransaction(isolationLevel);
        OpenedTransactionId = Guid.NewGuid().ToString();
        IsTransactionOpened = true;
    }

    public void CloseTransaction()
    {
        var transaction = this._connection.Transaction;

        if (transaction == null)
            return;

        transaction.Commit();
        transaction.Dispose();

        OpenedTransactionId = string.Empty;
        IsTransactionOpened = false;
    }

    public void RollbackTransaction()
    {
        var transaction = this._connection.Transaction;

        if (transaction == null)
            return;

        transaction.Rollback();
        transaction.Dispose();

        OpenedTransactionId = string.Empty;
        IsTransactionOpened = false;
    }

    #endregion
}