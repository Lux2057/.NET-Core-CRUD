namespace CRUD.DAL;

#region << Using >>

using System.Collections.Generic;
using System.Linq;
using CRUD.Extensions;

#endregion

internal static class QueryableExt
{
    public static IQueryable<TEntity> ApplyOrderSpecifications<TEntity>(this IQueryable<TEntity> queryable, IEnumerable<OrderSpecification<TEntity>> orderSpecifications)
    {
        var orderSpecificationsArray = orderSpecifications.ToArrayOrEmpty();

        if (orderSpecificationsArray.Length == 0)
            return queryable;

        var orderSpecification = orderSpecificationsArray[0];
        var orderExpression = orderSpecification.OrderExpression();

        var orderedQueryable = orderSpecification.IsDesc ?
                                       queryable.OrderByDescending(orderExpression) :
                                       queryable.OrderBy(orderExpression);

        if (orderSpecificationsArray.Length > 1)
            for (var i = 1; i < orderSpecificationsArray.Length; i++)
            {
                orderSpecification = orderSpecificationsArray[i];
                orderExpression = orderSpecification.OrderExpression();

                orderedQueryable = orderSpecification.IsDesc ?
                                           orderedQueryable.ThenByDescending(orderExpression) :
                                           orderedQueryable.ThenBy(orderExpression);
            }

        return orderedQueryable;
    }
}