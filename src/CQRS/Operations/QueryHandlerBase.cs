namespace CRUD.CQRS
{
    #region << Using >>

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using CRUD.DAL;
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    #endregion

    /// <summary>
    ///     Base Query handler which implements transaction scoped handling of a Query
    /// </summary>
    public abstract class QueryHandlerBase<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IQuery<TResponse>
    {
        #region Properties

        private readonly IUnitOfWork _unitOfWork;

        private readonly IValidator<TRequest> _validator;

        protected readonly IReadDispatcher Dispatcher;

        protected readonly IMapper Mapper;

        #endregion

        #region Constructors

        protected QueryHandlerBase(IServiceProvider serviceProvider)
        {
            this._unitOfWork = serviceProvider.GetService<IUnitOfWork>();
            this._validator = serviceProvider.GetService<IValidator<TRequest>>();
            this.Dispatcher = serviceProvider.GetService<IReadDispatcher>();
            this.Mapper = serviceProvider.GetService<IMapper>();
        }

        #endregion

        #region Interface Implementations

        public async Task<TResponse> Handle(TRequest query, CancellationToken cancellationToken)
        {
            if (this._validator != null)
                await this._validator.ValidateAndThrowAsync(query, cancellationToken);

            TResponse response;
            try
            {
                response = await Execute(query, cancellationToken);
            }
            catch (Exception)
            {
                await this._unitOfWork.RollbackCurrentTransactionScopeAsync();

                throw;
            }

            return response;
        }

        #endregion

        protected abstract Task<TResponse> Execute(TRequest request, CancellationToken cancellationToken);

        protected IReadRepository<TEntity> Repository<TEntity>() where TEntity : class, new()
        {
            return this._unitOfWork.ReadRepository<TEntity>();
        }
    }
}