namespace CRUD.Core
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using CRUD.DAL;
    using LinqSpecs;

    #endregion

    public class EntityByIdSpec<TEntity> : Specification<TEntity> where TEntity : EntityBase
    {
        #region Properties

        private readonly object id;

        #endregion

        #region Constructors

        public EntityByIdSpec(object id)
        {
            this.id = id;
        }

        #endregion

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            return x => x.Id == this.id;
        }
    }
}