﻿namespace CRUD.CQRS;

#region << Using >>

using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using CRUD.DAL;
using CRUD.Extensions;
using MediatR;

#endregion

/// <summary>
///     MediatR based implementation of the IDispatcher interface
/// </summary>
public class DefaultDispatcher : IDispatcher, IDisposable
{
    #region Properties

    private readonly IMediator _mediator;

    private readonly IScopedUnitOfWork scopedUnitOfWork;

    #endregion

    #region Constructors

    public DefaultDispatcher(IMediator mediator, IScopedUnitOfWork scopedUnitOfWork)
    {
        this._mediator = mediator;
        this.scopedUnitOfWork = scopedUnitOfWork;
    }

    #endregion

    #region Interface Implementations

    public async Task PushAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : CommandBase
    {
        if (!this.scopedUnitOfWork.IsOpened)
            this.scopedUnitOfWork.OpenTransactionScope(IsolationLevel.ReadCommitted);

        try
        {
            await this._mediator.Publish(notification: command, cancellationToken: cancellationToken);
        }
        catch
        {
            this.scopedUnitOfWork.RollbackChanges();
            throw;
        }
    }

    public async Task<TResponse> QueryAsync<TResponse>(QueryBase<TResponse> queryBase, CancellationToken cancellationToken = default)
    {
        try
        {
            return await this._mediator.Send(request: queryBase, cancellationToken: cancellationToken);
        }
        catch
        {
            this.scopedUnitOfWork.RollbackChanges();
            throw;
        }
    }

    public void Dispose()
    {
        this.scopedUnitOfWork.CloseTransactionScope();
    }

    #endregion
}