namespace CRUD.DAL
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public static class EnumerableExt
    {
        #region Constants

        private const int defaultPage = 1;

        private const int defaultPageSize = 50;

        #endregion

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

        public static IQueryable<TEntity> GetPage<TEntity>(this IQueryable<TEntity> enumerable, int totalCount, int? page, int? pageSize)
        {
            var currentPageSize = new[] { pageSize.GetValueOrDefault(defaultPageSize), 1 }.Max();
            var maxPage = new[] { 1, (int)Math.Ceiling((decimal)totalCount / currentPageSize) }.Max();
            var currentPage = new[] { new[] { page.GetValueOrDefault(defaultPage), 1 }.Max(), maxPage }.Min() - 1;

            return enumerable.Skip(currentPageSize * currentPage).Take(currentPageSize);
        }
    }
}