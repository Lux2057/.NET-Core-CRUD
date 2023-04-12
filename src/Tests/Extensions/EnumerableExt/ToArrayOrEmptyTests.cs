namespace CRUD.Tests;

#region << Using >>

using CRUD.Extensions;

#endregion

public class ToArrayOrEmptyTests
{
    public static IEnumerable<object?[]> AssertionData()
    {
        yield return new object[] { null, Array.Empty<object>() };
        yield return new object[] { new int[] { }, Array.Empty<int>() };
        yield return new object[] { new List<string>(), Array.Empty<string>() };
        yield return new object[] { new List<int> { 1, 2, 3, 4 }, new[] { 1, 2, 3, 4 } };
        yield return new object[] { new[] { 1, 2, 3, 4 }, new[] { 1, 2, 3, 4 } };
        yield return new object[] { new Queue<string>(new[] { "", "1", "2" }), new[] { "", "1", "2" } };
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public void Should_be_equal_to_expected<T>(IEnumerable<T> enumerable, T[] expected)
    {
        Assert.Equal(enumerable.ToArrayOrEmpty(), expected);
    }
}