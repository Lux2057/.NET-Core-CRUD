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

        public async Task<TResponse> QueryAsync<TResponse>(IQuery<TResponse> queryBase, CancellationToken cancellationToken = default)
        {
            return await this._mediator.Send(queryBase, cancellationToken);
        }

        #endregion
    }
}