namespace CRUD.CQRS
{
    #region << Using >>

    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    #endregion

    public class DefaultReadDispatcher : IReadDispatcher
    {
        #region Properties

        private readonly IMediator _mediator;

        #endregion

        #region Constructors

        public DefaultReadDispatcher(IMediator mediator)
        {
            this._mediator = mediator;
        }

        #endregion

        #region Interface Implementations

        public async Task<TResponse> QueryAsync<TQuery, TResponse>(TQuery queryBase, CancellationToken cancellationToken = default) where TQuery : QueryBase<TResponse>, new()
        {
            var response = await this._mediator.Send(queryBase, cancellationToken);
            queryBase.Result = response;

            return response;
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