namespace CRUD.CQRS
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

        public async Task PushAsync<TCommand>(TCommand commandBase, CancellationToken cancellationToken = default) where TCommand : CommandBase, new()
        {
            await this._mediator.Publish(commandBase, cancellationToken);
        }

        public void PushSync<TCommand>(TCommand commandBase) where TCommand : CommandBase, new()
        {
            this._mediator.Publish(commandBase).Wait();
        }

        public async Task<TResponse> QueryAsync<TQuery, TResponse>(TQuery queryBase, CancellationToken cancellationToken = default) where TQuery : QueryBase<TResponse>, new()
        {
            return await this._mediator.Send(queryBase, cancellationToken);
        }

        public TResponse QuerySync<TQuery, TResponse>(TQuery queryBase) where TQuery : QueryBase<TResponse>, new()
        {
            var task = this._mediator.Send(queryBase);
            task.Wait();

            return task.Result;
        }

        #endregion
    }
}