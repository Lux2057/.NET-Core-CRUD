namespace CRUD.Extensions
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public static class EnumerableExt
    {
        public static T[] ToArrayOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null ? Array.Empty<T>() : enumerable.ToArray();
        }
    }
}