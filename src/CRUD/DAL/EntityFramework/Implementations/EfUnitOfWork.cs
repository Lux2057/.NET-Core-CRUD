namespace CRUD.DAL.EntityFramework;

#region << Using >>

using System.Data;
using CRUD.DAL.Abstractions;
using Microsoft.EntityFrameworkCore;

#endregion

/// <summary>
///     EntityFrameworkCore based implementation of the IUnitOfWork interface.
/// </summary>
public class EfUnitOfWork : IUnitOfWork
{
    #region Properties

    public string OpenedTransactionId { get; private set; }

    public bool IsTransactionOpened { get; private set; }

    private readonly IEfDbContext _dbContext;

    #endregion

    #region Constructors

    public EfUnitOfWork(IEfDbContext efDbContext)
    {
        this._dbContext = efDbContext;
    }

    #endregion

    #region Interface Implementations

    public void OpenTransaction(IsolationLevel isolationLevel)
    {
        if (this._dbContext.Database.CurrentTransaction != null)
            return;

        var transaction = this._dbContext.Database.BeginTransaction(isolationLevel);
        OpenedTransactionId = transaction.TransactionId.ToString();
        IsTransactionOpened = true;
    }

    public void CloseTransaction()
    {
        if (this._dbContext.Database.CurrentTransaction == null)
            return;

        this._dbContext.Database.CurrentTransaction.Commit();
        OpenedTransactionId = string.Empty;
        IsTransactionOpened = false;
    }

    public void RollbackTransaction()
    {
        if (this._dbContext.Database.CurrentTransaction == null)
            return;

        this._dbContext.Database.CurrentTransaction.Rollback();
        OpenedTransactionId = string.Empty;
        IsTransactionOpened = false;
    }

    #endregion
}