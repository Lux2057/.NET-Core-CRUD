namespace CRUD.CQRS;

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

    private readonly IUnitOfWork _unitOfWork;

    private string _currentTransactionScopeId;

    #endregion

    #region Constructors

    public DefaultDispatcher(IMediator mediator, IUnitOfWork unitOfWork)
    {
        this._mediator = mediator;
        this._unitOfWork = unitOfWork;
    }

    #endregion

    #region Interface Implementations

    public async Task PushAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : CommandBase
    {
        if (this._currentTransactionScopeId.IsNullOrWhitespace())
            this._currentTransactionScopeId = await this._unitOfWork.BeginTransactionScopeAsync(IsolationLevel.ReadCommitted);

        try
        {
            await this._mediator.Publish(notification: command, cancellationToken: cancellationToken);
        }
        catch
        {
            await this._unitOfWork.RollbackCurrentTransactionScopeAsync();
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
            await this._unitOfWork.RollbackCurrentTransactionScopeAsync();
            throw;
        }
    }

    public void Dispose()
    {
        this._unitOfWork.EndTransactionScope(this._currentTransactionScopeId);
    }

    #endregion
}