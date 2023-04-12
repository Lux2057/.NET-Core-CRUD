namespace CRUD.Tests;

#region << Using >>

using CRUD.Extensions;

#endregion

public class ToJsonStringTests
{
    #region Nested Classes

    public class TestClass
    {
        #region Properties

        public int Number { get; set; }

        public string Str { get; set; }

        #endregion
    }

    #endregion

    public static IEnumerable<object[]> AssertionData()
    {
        yield return new object[]
                     {
                             new TestClass
                             {
                                     Str = "1",
                                     Number = 1
                             },
                             @"{""Number"":1,""Str"":""1""}"
                     };
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public void Should_be_equal_to_expected<T>(T value, string expected)
    {
        Assert.Equal(expected, value.ToJsonString());
    }
}