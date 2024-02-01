namespace Samples.ToDo.API.Projects;

#region << Using >>

using AutoMapper.QueryableExtensions;
using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

#endregion

public class GetProjectsQuery : QueryBase<ProjectDto[]>
{
    #region Properties

    public int UserId { get; }

    #endregion

    #region Constructors

    public GetProjectsQuery(int userId)
    {
        UserId = userId;
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
            var projects = await Repository.Read(new UserIdProp.FindBy.EqualTo<ProjectEntity>(request.UserId))
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