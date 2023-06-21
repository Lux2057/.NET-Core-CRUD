namespace CRUD.Extensions;

public static class EnumerableExt
{
    #region Constants

    public const int defaultPage = 1;

    public const int defaultPageSize = 10;

    #endregion

    /// <summary>
    ///     Returns an array which contains elements from the input sequence or
    ///     an empty array in case when the sequence is null or empty
    /// </summary>
    public static T[] ToArrayOrEmpty<T>(this IEnumerable<T> enumerable)
    {
        return enumerable == null ? Array.Empty<T>() : enumerable.ToArray();
    }

    /// <summary>
    ///     Returns an distinct array which contains elements from the input sequence or
    ///     an empty array in case when the sequence is null or empty
    /// </summary>
    public static T[] ToDistinctArrayOrEmpty<T>(this IEnumerable<T> enumerable)
    {
        return enumerable?.Distinct().ToArrayOrEmpty() ?? Array.Empty<T>();
    }

    /// <summary>
    ///     Returns a page IQueryable basing on LINQ Skip/Take functions
    /// </summary>
    public static IQueryable<TEntity> ToPage<TEntity>(this IQueryable<TEntity> queryable, int totalCount, int? page, int? pageSize)
    {
        var currentPageSize = new[] { pageSize.GetValueOrDefault(defaultPageSize), 1 }.Max();
        var maxPage = new[] { 1, (int)Math.Ceiling((decimal)totalCount / currentPageSize) }.Max();
        var currentPage = new[] { new[] { page.GetValueOrDefault(defaultPage), 1 }.Max(), maxPage }.Min() - 1;

        return queryable.Skip(currentPageSize * currentPage).Take(currentPageSize);
    }
}