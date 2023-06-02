namespace CRUD.CQRS
{
    #region << Using >>

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using CRUD.DAL.Abstractions;
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    #endregion

    /// <summary>
    ///     Base Query handler which implements transaction scoped handling of a Query
    /// </summary>
    public abstract class QueryHandlerBase<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : QueryBase<TResponse>
    {
        #region Properties

        protected IReadRepository Repository { get; }

        protected IReadDispatcher Dispatcher { get; }

        protected IMapper Mapper { get; }

        private readonly IValidator<TRequest> _validator;

        #endregion

        #region Constructors

        protected QueryHandlerBase(IServiceProvider serviceProvider)
        {
            this._validator = serviceProvider.GetService<IValidator<TRequest>>();
            Repository = serviceProvider.GetService<IReadRepository>();
            Dispatcher = serviceProvider.GetService<IReadDispatcher>();
            Mapper = serviceProvider.GetService<IMapper>();
        }

        #endregion

        #region Interface Implementations

        public async Task<TResponse> Handle(TRequest query, CancellationToken cancellationToken)
        {
            if (this._validator != null)
                await this._validator.ValidateAndThrowAsync(query, cancellationToken);

            return await Execute(query, cancellationToken);
        }

        #endregion

        protected abstract Task<TResponse> Execute(TRequest request, CancellationToken cancellationToken);
    }
}