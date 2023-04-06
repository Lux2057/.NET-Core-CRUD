﻿namespace CRUD.CQRS
{
    #region << Using >>

    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    #endregion

    public class DefaultReadWriteDispatcher : IReadWriteDispatcher
    {
        #region Properties

        private readonly IMediator _mediator;

        #endregion

        #region Constructors

        public DefaultReadWriteDispatcher(IMediator mediator)
        {
            this._mediator = mediator;
        }

        #endregion

        #region Interface Implementations

        public async Task PushAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : CommandBase
        {
            await this._mediator.Publish(notification: command, cancellationToken: cancellationToken);
        }

        public void PushSync<TCommand>(TCommand command) where TCommand : CommandBase
        {
            this._mediator.Publish(command).Wait();
        }

        public async Task<TResponse> QueryAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
        {
            return await this._mediator.Send(request: query, cancellationToken: cancellationToken);
        }

        public TResponse QuerySync<TResponse>(IQuery<TResponse> query)
        {
            var task = this._mediator.Send(query);
            task.Wait();

            return task.Result;
        }

        #endregion
    }
}