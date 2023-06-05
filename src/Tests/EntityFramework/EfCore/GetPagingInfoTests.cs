namespace EfTests.Core;

#region << Using >>

using CRUD.Core;
using CRUD.CQRS;
using CRUD.Extensions;
using EfTests.Shared;

#endregion

public class GetPagingInfoTests : ReadDispatcherTest
{
    #region Constructors

    public GetPagingInfoTests(IReadDispatcher dispatcher, TestDbContext context)
            : base(dispatcher, context) { }

    #endregion

    public static IEnumerable<object[]> AssertionData()
    {
        var queryable = new[] { 1, 2, 3, 4, 5, 6 }.AsQueryable();
        var totalCount = queryable.Count();

        yield return new object[]
                     {
                             totalCount, 1, 2, new PagingInfoDto
                                               {
                                                       TotalItemsCount = totalCount,
                                                       CurrentPage = 1,
                                                       PageSize = 2,
                                                       TotalPages = 3
                                               }.ToJsonString()
                     };

        yield return new object[]
                     {
                             totalCount, 2, 2, new PagingInfoDto
                                               {
                                                       TotalItemsCount = totalCount,
                                                       CurrentPage = 2,
                                                       PageSize = 2,
                                                       TotalPages = 3
                                               }.ToJsonString()
                     };

        yield return new object[]
                     {
                             totalCount, 3, 2, new PagingInfoDto
                                               {
                                                       TotalItemsCount = totalCount,
                                                       CurrentPage = 3,
                                                       PageSize = 2,
                                                       TotalPages = 3
                                               }.ToJsonString()
                     };

        yield return new object[]
                     {
                             totalCount, 0, 2, new PagingInfoDto
                                               {
                                                       TotalItemsCount = totalCount,
                                                       CurrentPage = 1,
                                                       PageSize = 2,
                                                       TotalPages = 3
                                               }.ToJsonString()
                     };

        yield return new object[]
                     {
                             totalCount, -1, 2, new PagingInfoDto
                                                {
                                                        TotalItemsCount = totalCount,
                                                        CurrentPage = 1,
                                                        PageSize = 2,
                                                        TotalPages = 3
                                                }.ToJsonString()
                     };

        yield return new object[]
                     {
                             totalCount, 4, 2, new PagingInfoDto
                                               {
                                                       TotalItemsCount = totalCount,
                                                       CurrentPage = 3,
                                                       PageSize = 2,
                                                       TotalPages = 3
                                               }.ToJsonString()
                     };

        yield return new object[]
                     {
                             totalCount, 1, 4, new PagingInfoDto
                                               {
                                                       TotalItemsCount = totalCount,
                                                       CurrentPage = 1,
                                                       PageSize = 4,
                                                       TotalPages = 2
                                               }.ToJsonString()
                     };

        yield return new object[]
                     {
                             totalCount, 2, 4, new PagingInfoDto
                                               {
                                                       TotalItemsCount = totalCount,
                                                       CurrentPage = 2,
                                                       PageSize = 4,
                                                       TotalPages = 2
                                               }.ToJsonString()
                     };

        yield return new object[]
                     {
                             totalCount, 3, 4, new PagingInfoDto
                                               {
                                                       TotalItemsCount = totalCount,
                                                       CurrentPage = 2,
                                                       PageSize = 4,
                                                       TotalPages = 2
                                               }.ToJsonString()
                     };

        yield return new object[]
                     {
                             totalCount, 1, 0, new PagingInfoDto
                                               {
                                                       TotalItemsCount = totalCount,
                                                       CurrentPage = 1,
                                                       PageSize = 1,
                                                       TotalPages = 6
                                               }.ToJsonString()
                     };

        yield return new object[]
                     {
                             totalCount, 6, 0, new PagingInfoDto
                                               {
                                                       TotalItemsCount = totalCount,
                                                       CurrentPage = 6,
                                                       PageSize = 1,
                                                       TotalPages = 6
                                               }.ToJsonString()
                     };
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public async Task Should_be_equal_to_expected(int totalCount, int? page, int? pageSize, string expected)
    {
        var pagingInfo = await this.dispatcher.QueryAsync(new GetPagingInfoQuery(page, pageSize, totalCount));

        Assert.Equal(expected, pagingInfo.ToJsonString());
    }
}