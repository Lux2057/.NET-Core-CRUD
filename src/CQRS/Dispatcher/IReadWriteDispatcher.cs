namespace CRUD.CQRS
{
    #region << Using >>

    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    public interface IReadWriteDispatcher : IReadDispatcher
    {
        public Task PushAsync<TCommand>(TCommand commandBase, CancellationToken cancellationToken = default) where TCommand : CommandBase;

        public void PushSync<TCommand>(TCommand commandBase) where TCommand : CommandBase;
    }
}