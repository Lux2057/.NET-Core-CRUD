namespace CRUD.CQRS
{
    #region << Using >>

    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    #endregion

    public class ReadWriteDispatcher : ReadDispatcher, IReadWriteDispatcher
    {
        #region Constructors

        public ReadWriteDispatcher(IMediator mediator) : base(mediator) { }

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

        #endregion
    }
}