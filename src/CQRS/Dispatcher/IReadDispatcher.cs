namespace CRUD.CQRS
{
    #region << Using >>

    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    public interface IReadDispatcher
    {
        public Task<TResponse> QueryAsync<TQuery, TResponse>(TQuery queryBase, CancellationToken cancellationToken = default) where TQuery : QueryBase<TResponse>, new();

        public TResponse QuerySync<TQuery, TResponse>(TQuery queryBase) where TQuery : QueryBase<TResponse>, new();
    }
}