namespace CRUD.DAL
{
    #region << Using >>

    using System;
    using System.Data;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    #endregion

    /// <summary>
    ///     EntityFrameworkCore based implementation of the IUnitOfWork interface
    /// </summary>
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

        /// <summary>
        ///     Starts a transaction scope
        /// </summary>
        /// <returns>If a transaction scope doesn't exist returns transaction id, otherwise returns empty string</returns>
        public async Task<string> BeginTransactionScopeAsync(IsolationLevel isolationLevel)
        {
            if (this._dbContext.Database.CurrentTransaction != null)
                return string.Empty;

            await this._dbContext.Database.BeginTransactionAsync(isolationLevel);

            return this._dbContext.Database.CurrentTransaction!.TransactionId.ToString();
        }

        public async Task EndTransactionScopeAsync(string transactionId)
        {
            if (this._dbContext.Database.CurrentTransaction == null ||
                this._dbContext.Database.CurrentTransaction.TransactionId.ToString() != transactionId)
                return;

            await this._dbContext.Database.CurrentTransaction.CommitAsync();
        }

        public async Task RollbackCurrentTransactionScopeAsync()
        {
            if (this._dbContext.Database.CurrentTransaction == null)
                return;

            await this._dbContext.Database.CurrentTransaction.RollbackAsync();
        }

        #endregion
    }
}