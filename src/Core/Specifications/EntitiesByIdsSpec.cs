namespace CRUD.Core
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using CRUD.DAL;
    using CRUD.Extensions;
    using LinqSpecs;

    #endregion

    public class EntitiesByIdsSpec<TEntity> : Specification<TEntity> where TEntity : EntityBase
    {
        #region Properties

        private readonly object[] ids;

        #endregion

        #region Constructors

        public EntitiesByIdsSpec(IEnumerable<object> ids)
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