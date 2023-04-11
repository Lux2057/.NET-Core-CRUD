namespace CRUD.DAL
{
    #region << Using >>

    using System;
    using System.Data;
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

        public IReadRepository<TEntity> ReadRepository<TEntity>() where TEntity : class, new()
        {
            return this._serviceProvider.GetService<IReadRepository<TEntity>>();
        }

        public IReadWriteRepository<TEntity> ReadWriteRepository<TEntity>() where TEntity : class, new()
        {
            return this._serviceProvider.GetService<IReadWriteRepository<TEntity>>();
        }

        public async Task BeginTransactionAsync(PermissionType permissionType)
        {
            if (this._dbContext.Database.CurrentTransaction != null)
                return;

            var isolationLevel = permissionType switch
            {
                    PermissionType.Read => IsolationLevel.ReadUncommitted,
                    PermissionType.ReadWrite => IsolationLevel.ReadCommitted,
                    _ => throw new NotImplementedException($"{nameof(PermissionType)} -> {permissionType}")
            };

            await this._dbContext.Database.BeginTransactionAsync(isolationLevel);
        }

        public async Task EndTransactionAsync()
        {
            if (this._dbContext.Database.CurrentTransaction == null)
                return;

            await this._dbContext.Database.CurrentTransaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            if (this._dbContext.Database.CurrentTransaction == null)
                return;

            await this._dbContext.Database.CurrentTransaction.RollbackAsync();
        }

        #endregion
    }
}