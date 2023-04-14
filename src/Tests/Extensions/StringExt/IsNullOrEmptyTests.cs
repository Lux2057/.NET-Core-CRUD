namespace Tests.Extensions;

#region << Using >>

using CRUD.Extensions;

#endregion

public class IsNullOrEmptyTests
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_be_empty(string str)
    {
        Assert.True(str.IsNullOrEmpty());
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("ewfwfwefwef")]
    [InlineData("123-wef")]
    public void Should_not_be_empty(string str)
    {
        Assert.False(str.IsNullOrEmpty());
    }
}