namespace CRUD.DAL
{
    #region << Using >>

    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.Infrastructure;

    #endregion

    public interface IEfDbContext
    {
        #region Properties

        public DatabaseFacade Database { get; }

        public ChangeTracker ChangeTracker { get; }

        #endregion

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        public DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}