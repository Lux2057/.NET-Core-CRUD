namespace Samples.ToDo.API;

#region << Using >>

using AutoMapper.QueryableExtensions;
using CRUD.Core;
using CRUD.CQRS;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Samples.ToDo.Shared;

#endregion

public class ReadProjectsQuery : QueryBase<PaginatedResponseDto<ProjectDto>>
{
    #region Properties

    public bool DisablePaging { get; }

    public int Page { get; }

    public int PageSize { get; }

    public int UserId { get; }

    #endregion

    #region Constructors

    public ReadProjectsQuery(int userId,
                             bool disablePaging,
                             int? page = default,
                             int? pageSize = default)
    {
        UserId = userId;
        DisablePaging = disablePaging;
        Page = page ?? 1;
        PageSize = pageSize ?? 20;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Handler : QueryHandlerBase<ReadProjectsQuery, PaginatedResponseDto<ProjectDto>>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task<PaginatedResponseDto<ProjectDto>> Execute(ReadProjectsQuery request, CancellationToken cancellationToken)
        {
            var projectsSpec = new IsDeletedProp.FindBy.EqualTo<ProjectEntity>(false) &&
                               new UserIdProp.FindBy.EqualTo<ProjectEntity>(request.UserId);

            var projectsQueryable = Repository.Read(projectsSpec).ProjectTo<ProjectDto>(Mapper.ConfigurationProvider);
            var totalCount = await projectsQueryable.CountAsync(cancellationToken);

            if (totalCount == 0)
                return new PaginatedResponseDto<ProjectDto>
                       {
                               Items = Array.Empty<ProjectDto>(),
                               PagingInfo = new PagingInfoDto
                                            {
                                                    TotalItemsCount = 0,
                                                    PageSize = request.PageSize,
                                                    TotalPages = 0,
                                                    CurrentPage = request.Page
                                            }
                       };

            PagingInfoDto pagingInfo;
            if (!request.DisablePaging)
            {
                pagingInfo = await Dispatcher.QueryAsync(new GetPagingInfoQuery(request.Page, request.PageSize, totalCount), cancellationToken);

                projectsQueryable = projectsQueryable.Skip(pagingInfo.PageSize * (pagingInfo.CurrentPage - 1)).Take(pagingInfo.PageSize);
            }
            else
            {
                pagingInfo = new PagingInfoDto
                             {
                                     TotalItemsCount = totalCount,
                                     PageSize = totalCount,
                                     TotalPages = 1,
                                     CurrentPage = 1
                             };
            }

            var projects = await projectsQueryable.ToArrayAsync(cancellationToken);

            return new PaginatedResponseDto<ProjectDto>
                   {
                           Items = projects,
                           PagingInfo = pagingInfo
                   };
        }
    }

    #endregion
}