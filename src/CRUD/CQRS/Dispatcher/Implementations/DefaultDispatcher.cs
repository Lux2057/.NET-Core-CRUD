namespace CRUD.CQRS;

#region << Using >>

using System.Data;
using System.Threading;
using System.Threading.Tasks;
using CRUD.DAL.Abstractions;
using MediatR;

#endregion

/// <summary>
///     MediatR based implementation of the IDispatcher interface.
/// </summary>
public class DefaultDispatcher : IDispatcher
{
    #region Properties

    private readonly IMediator _mediator;

    private readonly IUnitOfWork unitOfWork;

    #endregion

    #region Constructors

    public DefaultDispatcher(IMediator mediator, IUnitOfWork unitOfWork)
    {
        this._mediator = mediator;
        this.unitOfWork = unitOfWork;
    }

    #endregion

    #region Interface Implementations

    public void Dispose()
    {
        this.unitOfWork.CloseTransaction();
    }

    public async Task PushAsync<TCommand>(TCommand command,
                                          CancellationToken cancellationToken = default,
                                          IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) where TCommand : CommandBase
    {
        if (!this.unitOfWork.IsTransactionOpened)
            this.unitOfWork.OpenTransaction(isolationLevel);

        try
        {
            await this._mediator.Publish(notification: command, cancellationToken: cancellationToken);
        }
        catch
        {
            this.unitOfWork.RollbackTransaction();
            throw;
        }
    }

    public async Task<TResponse> QueryAsync<TResponse>(QueryBase<TResponse> queryBase,
                                                       CancellationToken cancellationToken = default,
                                                       IsolationLevel? isolationLevel = null)
    {
        if (isolationLevel != null && !this.unitOfWork.IsTransactionOpened)
            this.unitOfWork.OpenTransaction(isolationLevel.Value);

        try
        {
            return await this._mediator.Send(request: queryBase, cancellationToken: cancellationToken);
        }
        catch
        {
            this.unitOfWork.RollbackTransaction();
            throw;
        }
    }

    #endregion
}