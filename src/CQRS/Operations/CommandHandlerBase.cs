namespace CRUD.CQRS
{
    #region << Using >>

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Data;
    using AutoMapper;
    using CRUD.DAL;
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    #endregion

    public abstract class CommandHandlerBase<TNotification> : INotificationHandler<TNotification> where TNotification : CommandBase
    {
        #region Properties

        private readonly IUnitOfWork _unitOfWork;

        private readonly IValidator<TNotification> _validator;

        protected readonly IReadWriteDispatcher Dispatcher;

        protected readonly IMapper Mapper;

        #endregion

        #region Constructors

        protected CommandHandlerBase(IServiceProvider serviceProvider)
        {
            this._unitOfWork = serviceProvider.GetService<IUnitOfWork>();
            this._validator = serviceProvider.GetService<IValidator<TNotification>>();
            this.Dispatcher = serviceProvider.GetService<IReadWriteDispatcher>();
            this.Mapper = serviceProvider.GetService<IMapper>();
        }

        #endregion

        #region Interface Implementations

        public async Task Handle(TNotification command, CancellationToken cancellationToken)
        {
            if (this._validator != null)
                await this._validator.ValidateAndThrowAsync(command, cancellationToken);

            try
            {
                var transactionId = await this._unitOfWork.BeginTransactionScopeAsync(IsolationLevel.ReadCommitted);

                await Execute(command, cancellationToken);

                await this._unitOfWork.EndTransactionScopeAsync(transactionId);
            }
            catch (Exception)
            {
                await this._unitOfWork.RollbackCurrentTransactionScopeAsync();

                throw;
            }
        }

        #endregion

        protected abstract Task Execute(TNotification command, CancellationToken cancellationToken);

        protected IReadWriteRepository<TEntity> Repository<TEntity>() where TEntity : class, new()
        {
            return this._unitOfWork.ReadWriteRepository<TEntity>();
        }
    }
}