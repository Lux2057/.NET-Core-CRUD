namespace CRUD.DAL
{
    #region << Using >>

    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using LinqSpecs;

    #endregion

    public interface IReadRepository<TEntity> where TEntity : class, new()
    {
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicateExpression = null);

        IQueryable<TEntity> Get(Specification<TEntity> specification);
    }
}