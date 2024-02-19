namespace Samples.ToDo.API.Projects;

#region << Using >>

using AutoMapper.QueryableExtensions;
using CRUD.Core;
using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using Extensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Samples.ToDo.Shared;

#endregion

public class GetProjectsQuery : QueryBase<PaginatedResponseDto<ProjectDto>>
{
    #region Properties

    public bool DisablePaging { get; }

    public int Page { get; }

    public int PageSize { get; }

    public int UserId { get; }

    public string SearchTerm { get; }

    public int[] TagsIds { get; }

    #endregion

    #region Constructors

    public GetProjectsQuery(int userId,
                            string searchTerm,
                            IEnumerable<int> tagsIds,
                            bool disablePaging,
                            int? page = default,
                            int? pageSize = default)
    {
        UserId = userId;
        DisablePaging = disablePaging;
        Page = page ?? 1;
        PageSize = pageSize ?? 10;
        SearchTerm = searchTerm?.Trim();
        TagsIds = tagsIds.ToDistinctArrayOrEmpty();
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Handler : QueryHandlerBase<GetProjectsQuery, PaginatedResponseDto<ProjectDto>>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task<PaginatedResponseDto<ProjectDto>> Execute(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            var projectsSpec = new IsDeletedProp.FindBy.EqualTo<ProjectEntity>(false) &&
                               new UserIdProp.FindBy.EqualTo<ProjectEntity>(request.UserId) &&
                               (new NameProp.FindBy.ContainedTerm<ProjectEntity>(request.SearchTerm) ||
                                new DescriptionProp.FindBy.ContainedTerm<ProjectEntity>(request.SearchTerm));

            if (request.TagsIds.Any())
            {
                var projectsIds = await Repository.Read(new TagIdProp.FindBy.ContainedIn<ProjectToTagEntity>(request.TagsIds))
                                                  .Select(r => r.ProjectId).Distinct().ToArrayAsync(cancellationToken);

                projectsSpec = projectsSpec && new FindEntitiesByIds<ProjectEntity, int>(projectsIds);
            }

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

            var tagsMaps = await Repository.Read(new ProjectIdProp.FindBy.ContainedIn<ProjectToTagEntity>(projects.Select(r => r.Id)) &&
                                                 new UserIdProp.FindBy.EqualTo<ProjectToTagEntity>(request.UserId))
                                           .Select(r => new
                                                        {
                                                                r.TagId,
                                                                r.ProjectId
                                                        }).ToArrayAsync(cancellationToken);

            if (tagsMaps.Length > 0)
            {
                var tags = await Repository.Read(new FindEntitiesByIds<TagEntity, int>(tagsMaps.Select(r => r.TagId)))
                                           .ProjectTo<TagDto>(Mapper.ConfigurationProvider)
                                           .ToArrayAsync(cancellationToken);

                Parallel.ForEach(tagsMaps.GroupBy(r => r.ProjectId),
                                 tagsGroup =>
                                 {
                                     var project = projects.Single(r => r.Id == tagsGroup.Key);
                                     var currentTagsIds = tagsGroup.Select(r => r.TagId).ToArray();

                                     project.Tags = tags.Where(r => currentTagsIds.Contains(r.Id)).ToArray();
                                 });
            }

            return new PaginatedResponseDto<ProjectDto>
                   {
                           Items = projects,
                           PagingInfo = pagingInfo
                   };
        }
    }

    #endregion
}