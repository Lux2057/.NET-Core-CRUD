namespace CRUD.CQRS
{
    #region << Using >>

    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    #endregion

    public class ReadWriteDispatcher : IReadWriteDispatcher
    {
        #region Properties

        private readonly IMediator _mediator;

        #endregion

        #region Constructors

        public ReadWriteDispatcher(IMediator mediator)
        {
            this._mediator = mediator;
        }

        #endregion

        #region Interface Implementations

        public async Task PushAsync(ICommand command, CancellationToken cancellationToken = default)
        {
            await this._mediator.Publish(command, cancellationToken);
        }

        public void PushSync(ICommand command)
        {
            this._mediator.Publish(command).Wait();
        }

        public async Task<TResponse> QueryAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
        {
            var response = await this._mediator.Send(query, cancellationToken);
            query.Result = response;

            return response;
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