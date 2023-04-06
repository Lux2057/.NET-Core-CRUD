namespace CRUD.CQRS
{
    #region << Using >>

    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    #endregion

    public class ReadDispatcher : IReadDispatcher
    {
        #region Properties

        protected readonly IMediator _mediator;

        #endregion

        #region Constructors

        public ReadDispatcher(IMediator mediator)
        {
            this._mediator = mediator;
        }

        #endregion

        #region Interface Implementations

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