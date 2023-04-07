namespace CRUD.Core
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using CRUD.DAL;
    using LinqSpecs;

    #endregion

    public class EntitiesByIdsSpec<TEntity, TId> : Specification<TEntity> where TEntity : IId<TId>
    {
        #region Properties

        private readonly TId[] ids;

        #endregion

        #region Constructors

        public EntitiesByIdsSpec(IEnumerable<TId> ids)
        {
            this.ids = ids.ToArrayOrEmpty();
        }

        #endregion

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            if (!this.ids.Any())
                return x => true;

            return x => this.ids.Contains(x.Id);
        }
    }
}