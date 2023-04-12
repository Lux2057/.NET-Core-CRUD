namespace CRUD.Tests;

#region << Using >>

using CRUD.Extensions;

#endregion

public class ToPageTests
{
    public static IEnumerable<object[]> AssertionData()
    {
        var queryable = new[] { 1, 2, 3, 4, 5, 6 }.AsQueryable();
        var totalCount = queryable.Count();

        yield return new object[] { queryable, totalCount, 1, 2, new[] { 1, 2 } };
        yield return new object[] { queryable, totalCount, 2, 2, new[] { 3, 4 } };
        yield return new object[] { queryable, totalCount, 3, 2, new[] { 5, 6 } };

        yield return new object[] { queryable, totalCount, 0, 2, new[] { 1, 2 } };
        yield return new object[] { queryable, totalCount, -1, 2, new[] { 1, 2 } };
        yield return new object[] { queryable, totalCount, 4, 2, new[] { 5, 6 } };

        yield return new object[] { queryable, totalCount, 1, 4, new[] { 1, 2, 3, 4 } };
        yield return new object[] { queryable, totalCount, 2, 4, new[] { 5, 6 } };
        yield return new object[] { queryable, totalCount, 3, 4, new[] { 5, 6 } };

        yield return new object[] { queryable, totalCount, 1, 0, new[] { 1 } };
        yield return new object[] { queryable, totalCount, 6, 0, new[] { 6 } };
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public void Should_be_equal_to_expected<T>(IQueryable<T> entities, int totalCount, int? page, int? pageSize, T[] expected)
    {
        Assert.Equal(entities.ToPage(totalCount, page, pageSize).ToArray(), expected);
    }
}