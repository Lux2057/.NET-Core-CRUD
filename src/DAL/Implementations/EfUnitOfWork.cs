namespace CRUD.DAL
{
    #region << Using >>

    using System;
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    #endregion

    public class EfUnitOfWork : IUnitOfWork
    {
        #region Properties

        private readonly IEfDbContext _dbContext;

        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region Constructors

        public EfUnitOfWork(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            this._dbContext = serviceProvider.GetService<IEfDbContext>();
        }

        #endregion

        #region Interface Implementations

        public IReadRepository<TEntity, TId> ReadRepository<TEntity, TId>() where TEntity : IId<TId>, new()
        {
            return this._serviceProvider.GetService<IReadRepository<TEntity, TId>>();
        }

        public IReadWriteRepository<TEntity, TId> ReadWriteRepository<TEntity, TId>() where TEntity : IId<TId>, new()
        {
            return this._serviceProvider.GetService<IReadWriteRepository<TEntity, TId>>();
        }

        public async Task BeginTransactionAsync(PermissionType permissionType, CancellationToken cancellationToken = default)
        {
            if (this._dbContext.Database.CurrentTransaction != null)
                return;

            var isolationLevel = permissionType switch
            {
                    PermissionType.Read => IsolationLevel.ReadUncommitted,
                    PermissionType.ReadWrite => IsolationLevel.ReadCommitted,
                    _ => throw new NotImplementedException(nameof(PermissionType))
            };

            await this._dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
        }

        public async Task EndTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (this._dbContext.Database.CurrentTransaction == null)
                return;

            await this._dbContext.Database.CurrentTransaction.CommitAsync(cancellationToken);
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (this._dbContext.Database.CurrentTransaction == null)
                return;

            await this._dbContext.Database.CurrentTransaction.RollbackAsync(cancellationToken);
        }

        #endregion
    }
}