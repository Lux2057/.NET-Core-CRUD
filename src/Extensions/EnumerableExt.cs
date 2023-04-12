namespace CRUD.Extensions;

public static class EnumerableExt
{
    #region Constants

    public const int defaultPage = 1;

    public const int defaultPageSize = 10;

    #endregion

    public static T[] ToArrayOrEmpty<T>(this IEnumerable<T>? enumerable)
    {
        return enumerable == null ? Array.Empty<T>() : enumerable.ToArray();
    }

    public static IQueryable<TEntity> ToPage<TEntity>(this IQueryable<TEntity> queryable, int totalCount, int? page, int? pageSize)
    {
        var currentPageSize = new[] { pageSize.GetValueOrDefault(defaultPageSize), 1 }.Max();
        var maxPage = new[] { 1, (int)Math.Ceiling((decimal)totalCount / currentPageSize) }.Max();
        var currentPage = new[] { new[] { page.GetValueOrDefault(defaultPage), 1 }.Max(), maxPage }.Min() - 1;

        return queryable.Skip(currentPageSize * currentPage).Take(currentPageSize);
    }
}