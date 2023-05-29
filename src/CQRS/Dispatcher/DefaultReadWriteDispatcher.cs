namespace CRUD.CQRS
{
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
    ///     MediatR based implementation of IReadWriteDispatcher interface
    /// </summary>
    public class DefaultReadWriteDispatcher : IReadWriteDispatcher, IDisposable
    {
        #region Properties

        private readonly IMediator _mediator;

        readonly IUnitOfWork _unitOfWork;

        private string _currentTransactionScopeId;

        #endregion

        #region Constructors

        public DefaultReadWriteDispatcher(IMediator mediator, IUnitOfWork unitOfWork)
        {
            this._mediator = mediator;
            this._unitOfWork = unitOfWork;
        }

        #endregion

        #region Interface Implementations

        public void Dispose()
        {
            this._unitOfWork.EndTransactionScope(this._currentTransactionScopeId);
        }

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

        public async Task<TResponse> QueryAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
        {
            try
            {
                return await this._mediator.Send(request: query, cancellationToken: cancellationToken);
            }
            catch
            {
                await this._unitOfWork.RollbackCurrentTransactionScopeAsync();
                throw;
            }
        }

        #endregion
    }
}