namespace Samples.ToDo.API.Projects;

#region << Using >>

using AutoMapper.QueryableExtensions;
using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using Extensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

#endregion

public class GetProjectsQuery : QueryBase<ProjectDto[]>
{
    #region Properties

    public int UserId { get; }

    public string SearchTerm { get; }

    public int[] TagsIds { get; }

    #endregion

    #region Constructors

    public GetProjectsQuery(int userId,
                            string searchTerm,
                            IEnumerable<int> tagsIds)
    {
        UserId = userId;
        SearchTerm = searchTerm.Trim();
        TagsIds = tagsIds.ToDistinctArrayOrEmpty();
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Handler : QueryHandlerBase<GetProjectsQuery, ProjectDto[]>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task<ProjectDto[]> Execute(GetProjectsQuery request, CancellationToken cancellationToken)
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

            var projects = await Repository.Read(projectsSpec)
                                           .ProjectTo<ProjectDto>(Mapper.ConfigurationProvider)
                                           .ToArrayAsync(cancellationToken);

            if (projects.Length == 0)
                return Array.Empty<ProjectDto>();

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

            return projects;
        }
    }

    #endregion
}