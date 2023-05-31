namespace CRUD.WebAPI
{
    #region << Using >>

    using CRUD.CQRS;
    using Microsoft.AspNetCore.Mvc;

    #endregion

    public abstract class DispatcherControllerBase : ControllerBase
    {
        #region Properties

        readonly IDispatcher _dispatcher;

        #endregion

        #region Constructors

        protected DispatcherControllerBase(IDispatcher dispatcher)
        {
            this._dispatcher = dispatcher;
        }

        #endregion

        protected async Task<TQueryResponse> QueryAsync<TQueryResponse>(QueryBase<TQueryResponse> query, CancellationToken cancellationToken = default)
        {
            return await this._dispatcher.QueryAsync(query, cancellationToken);
        }

        protected async Task PushAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : CommandBase
        {
            await this._dispatcher.PushAsync(command, cancellationToken);
        }
    }
}