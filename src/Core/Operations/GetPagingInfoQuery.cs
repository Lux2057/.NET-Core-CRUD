namespace CRUD.Core;

#region << Using >>

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CRUD.CQRS;
using JetBrains.Annotations;

#endregion

public class GetPagingInfoQuery : QueryBase<PagingInfoDto>
{
    #region Constants

    public const int defaultPage = 1;

    public const int defaultPageSize = 10;

    #endregion

    #region Properties

    public int? Page { get; }

    public int? PageSize { get; }

    public int TotalCount { get; }

    #endregion

    #region Constructors

    public GetPagingInfoQuery(int? page, int? pageSize, int totalCount)
    {
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    public class Handler : QueryHandlerBase<GetPagingInfoQuery, PagingInfoDto>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task<PagingInfoDto> Execute(GetPagingInfoQuery request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
                                  {
                                      var pageSize = new[] { request.PageSize.GetValueOrDefault(defaultPageSize), 1 }.Max();
                                      var totalPages = new[] { 1, (int)Math.Ceiling((decimal)request.TotalCount / pageSize) }.Max();
                                      var currentPage = new[] { new[] { request.Page.GetValueOrDefault(defaultPage), 1 }.Max(), totalPages }.Min();

                                      return new PagingInfoDto
                                             {
                                                     CurrentPage = currentPage,
                                                     TotalPages = totalPages,
                                                     TotalItemsCount = request.TotalCount,
                                                     PageSize = pageSize
                                             };
                                  }, cancellationToken);
        }
    }

    #endregion
}