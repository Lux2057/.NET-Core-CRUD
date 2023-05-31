namespace CRUD.CQRS
{
    #region << Using >>

    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    /// <summary>
    ///     A dispatcher interface to perform read-based operations
    /// </summary>
    public interface IReadDispatcher
    {
        public Task<TResponse> QueryAsync<TResponse>(QueryBase<TResponse> queryBase, CancellationToken cancellationToken = default);
    }
}