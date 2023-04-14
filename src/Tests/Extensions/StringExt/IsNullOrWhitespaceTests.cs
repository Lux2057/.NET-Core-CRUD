namespace Tests.Extensions;

#region << Using >>

using CRUD.Extensions;

#endregion

public class IsNullOrWhitespaceTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Should_be_empty(string str)
    {
        Assert.True(str.IsNullOrWhitespace());
    }

    [Theory]
    [InlineData("ewfwfwefwef")]
    [InlineData("123-wef")]
    public void Should_not_be_empty(string str)
    {
        Assert.False(str.IsNullOrWhitespace());
    }
}