namespace CRUD.DAL
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using CRUD.Extensions;

    #endregion

    public static class EnumerableExt
    {
        public static object[] GetIds<TEntity>(this IEnumerable<TEntity> enumerable) where TEntity : EntityBase
        {
            return enumerable.Select(r => (object)r.Id).ToArrayOrEmpty();
        }
    }
}