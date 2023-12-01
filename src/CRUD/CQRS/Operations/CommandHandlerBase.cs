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
    ///     Base Command handler which implements transaction scoped handling of a Command.
    /// </summary>
    public abstract class CommandHandlerBase<TNotification> : INotificationHandler<TNotification> where TNotification : CommandBase
    {
        #region Properties

        protected IRepository Repository { get; }

        protected IDispatcher Dispatcher { get; }

        protected IMapper Mapper { get; }

        private readonly IValidator<TNotification> _validator;

        #endregion

        #region Constructors

        protected CommandHandlerBase(IServiceProvider serviceProvider)
        {
            this._validator = serviceProvider.GetService<IValidator<TNotification>>();
            Repository = serviceProvider.GetService<IRepository>();
            Dispatcher = serviceProvider.GetService<IDispatcher>();
            Mapper = serviceProvider.GetService<IMapper>();
        }

        #endregion

        #region Interface Implementations

        public async Task Handle(TNotification command, CancellationToken cancellationToken)
        {
            if (this._validator != null)
                await this._validator.ValidateAndThrowAsync(command, cancellationToken);

            await Execute(command, cancellationToken);
        }

        #endregion

        protected abstract Task Execute(TNotification command, CancellationToken cancellationToken);
    }
}