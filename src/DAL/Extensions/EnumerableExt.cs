namespace CRUD.DAL
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public static class EnumerableExt
    {
        public static IOrderedEnumerable<TEntity> OrderBy<TEntity, TProperty>(this IEnumerable<TEntity> enumerable, OrderSpecification<TEntity, TProperty> specification)
        {
            var ordering = specification.Expression.Compile();

            return specification.Type switch
            {
                    OrderType.Ascending => enumerable.OrderBy(ordering),
                    OrderType.Descending => enumerable.OrderByDescending(ordering),
                    _ => throw new NotImplementedException($"{typeof(OrderType)}: {specification.Type}")
            };
        }

        public static IOrderedEnumerable<TEntity> ThenBy<TEntity, TProperty>(this IOrderedEnumerable<TEntity> enumerable, OrderSpecification<TEntity, TProperty> specification)
        {
            var ordering = specification.Expression.Compile();

            return specification.Type switch
            {
                    OrderType.Ascending => enumerable.ThenBy(ordering),
                    OrderType.Descending => enumerable.ThenByDescending(ordering),
                    _ => throw new NotImplementedException($"{typeof(OrderType)}: {specification.Type}")
            };
        }
    }
}