namespace CRUD.CQRS
{
    #region << Using >>

    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using CRUD.DAL;
    using MediatR;

    #endregion

    public interface ICommandHandler<TNotification> : INotificationHandler<TNotification> where TNotification : INotification
    {
        #region Properties

        protected IReadWriteDispatcher Dispatcher { get; }

        protected IMapper Mapper { get; }

        #endregion

        protected Task Execute(TNotification command, CancellationToken cancellationToken);

        protected IReadWriteRepository<TEntity, TId> Repository<TEntity, TId>() where TEntity : IId<TId>, new();
    }
}