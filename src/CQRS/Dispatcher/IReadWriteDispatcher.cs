namespace CRUD.CQRS
{
    #region << Using >>

    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    /// <summary>
    ///     A dispatcher interface to perform read- and write-base operations
    /// </summary>
    public interface IReadWriteDispatcher : IReadDispatcher
    {
        public Task PushAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : CommandBase;
    }
}