namespace CRUD.Core
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using CRUD.DAL;

    #endregion

    public static class Enumerable
    {
        public static TId[] GetIds<TEntity, TId>(this IEnumerable<TEntity> entities) where TEntity : IId<TId>
        {
            return entities.Select(r => r.Id).ToArrayOrEmpty();
        }
    }
}