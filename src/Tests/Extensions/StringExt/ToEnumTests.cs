namespace CRUD.Tests;

#region << Using >>

using CRUD.Extensions;

#endregion

public class ToEnumTests
{
    #region Nested Classes

    public enum TestEnum
    {
        Value1 = 1,

        Test_Name2 = 2
    }

    #endregion

    public static IEnumerable<object[]> AssertionData()
    {
        yield return new object[] { "Value1", TestEnum.Value1 };
        yield return new object[] { "Test_Name2", TestEnum.Test_Name2 };
    }

    public static IEnumerable<object[]> ExceptionsData()
    {
        yield return new object[] { null };
        yield return new object[] { "VALUE1" };
        yield return new object[] { "TestName2" };
        yield return new object[] { "" };
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public void Should_be_equal_to_expected(string value, TestEnum expected)
    {
        Assert.Equal(value.ToEnum<TestEnum>(), expected);
    }

    [Theory]
    [MemberData(nameof(ExceptionsData))]
    public void Should_throw_exceptions(string value)
    {
        Assert.ThrowsAny<Exception>(() => value.ToEnum<TestEnum>());
    }
}