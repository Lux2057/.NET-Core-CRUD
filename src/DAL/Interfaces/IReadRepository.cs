namespace CRUD.DAL
{
    #region << Using >>

    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using LinqSpecs;

    #endregion

    public interface IReadRepository<TEntity, in TId> where TEntity : IId<TId>, new()
    {
        Task<TEntity> GetByIdOrDefaultAsync(TId id, CancellationToken cancellationToken = default);

        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicateExpression = null);

        IQueryable<TEntity> Get(Specification<TEntity> specification);
    }
}