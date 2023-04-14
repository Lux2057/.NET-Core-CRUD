namespace Tests.Extensions;

#region << Using >>

using CRUD.Extensions;

#endregion

public class ToJoinedStringTests
{
    public static IEnumerable<object[]> AssertionData()
    {
        yield return new object[] { null, null, "" };
        yield return new object[] { new string[] { }, null, "" };
        yield return new object[] { new List<string>(), string.Empty, "" };
        yield return new object[] { new Queue<string>(), "-", "" };
        yield return new object[] { new[] { "1", "2", "3", "4" }, null, "1234" };
        yield return new object[] { new[] { "1", "2", "3", "4" }, "-", "1-2-3-4" };
        yield return new object[] { new List<string> { "1", "2", "3", "4" }, "+", "1+2+3+4" };
        yield return new object[] { new Queue<string>(new[] { "1", "2", "3", "4" }), ":", "1:2:3:4" };
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public void Should_be_equal_to_expected(IEnumerable<string> enumerable, string separator, string expected)
    {
        Assert.Equal(expected, enumerable.ToJoinedString(separator));
    }
}