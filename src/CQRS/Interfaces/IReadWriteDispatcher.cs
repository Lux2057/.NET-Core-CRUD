namespace CRUD.CQRS
{
    #region << Using >>

    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    public interface IReadWriteDispatcher : IReadDispatcher
    {
        public Task PushAsync(ICommand command, CancellationToken cancellationToken = default);

        public void PushSync(ICommand command);
    }
}