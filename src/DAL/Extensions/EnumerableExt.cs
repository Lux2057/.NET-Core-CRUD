namespace CRUD.DAL
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using CRUD.Extensions;

    #endregion

    public static class EnumerableExt
    {
        public static object[] GetIds(this IEnumerable<EntityBase> enumerable)
        {
            return enumerable.Select(r => r.Id).ToArrayOrEmpty();
        }
    }
}