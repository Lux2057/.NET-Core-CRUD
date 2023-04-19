namespace CRUD.Core
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using LinqSpecs;

    #endregion

    /// <summary>
    ///     Finds an entity by specified id
    /// </summary>
    public class EntityByIdSpec<TEntity, TId> : Specification<TEntity> where TEntity : IId<TId>
    {
        #region Properties

        private readonly TId id;

        #endregion

        #region Constructors

        public EntityByIdSpec(TId id)
        {
            this.id = id;
        }

        #endregion

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            return x => Equals(x.Id, this.id);
        }
    }
}