namespace CRUD.CQRS
{
    #region << Using >>

    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using CRUD.DAL;
    using MediatR;

    #endregion

    public interface IQueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        #region Properties

        protected IReadDispatcher Dispatcher { get; }

        protected IMapper Mapper { get; }

        #endregion

        protected Task<TResponse> Execute(TRequest request, CancellationToken cancellationToken);

        protected IReadRepository<TEntity, TId> Repository<TEntity, TId>() where TEntity : IId<TId>, new();
    }
}