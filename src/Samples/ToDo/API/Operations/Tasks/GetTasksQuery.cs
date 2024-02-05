namespace Samples.ToDo.API;

#region << Using >>

using AutoMapper.QueryableExtensions;
using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

#endregion

public class GetTasksQuery : QueryBase<TaskDto[]>
{
    #region Properties

    public int UserId { get; }

    public int ProjectId { get; }

    #endregion

    #region Constructors

    public GetTasksQuery(int userId, int projectId)
    {
        UserId = userId;
        ProjectId = projectId;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Handler : QueryHandlerBase<GetTasksQuery, TaskDto[]>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task<TaskDto[]> Execute(GetTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await Repository.Read(new IsDeletedProp.FindBy.EqualTo<TaskEntity>(false) &&
                                              new UserIdProp.FindBy.EqualTo<TaskEntity>(request.UserId) &&
                                              new ProjectIdProp.FindBy.EqualTo<TaskEntity>(request.ProjectId))
                                        .ProjectTo<TaskDto>(Mapper.ConfigurationProvider)
                                        .ToArrayAsync(cancellationToken);

            if (tasks.Length == 0)
                return Array.Empty<TaskDto>();

            var tagsMaps = await Repository.Read(new TaskIdProp.FindBy.ContainedIn<TaskToTagEntity>(tasks.Select(r => r.Id)) &&
                                                 new UserIdProp.FindBy.EqualTo<TaskToTagEntity>(request.UserId))
                                           .Select(r => new
                                                        {
                                                                r.TagId,
                                                                r.TaskId
                                                        }).ToArrayAsync(cancellationToken);

            if (tagsMaps.Length > 0)
            {
                var tags = await Repository.Read(new FindEntitiesByIds<TagEntity, int>(tagsMaps.Select(r => r.TagId)))
                                           .ProjectTo<TagDto>(Mapper.ConfigurationProvider)
                                           .ToArrayAsync(cancellationToken);

                Parallel.ForEach(tagsMaps.GroupBy(r => r.TaskId),
                                 tagsGroup =>
                                 {
                                     var task = tasks.Single(r => r.Id == tagsGroup.Key);
                                     var currentTagsIds = tagsGroup.Select(r => r.TagId).ToArray();

                                     task.Tags = tags.Where(r => currentTagsIds.Contains(r.Id)).ToArray();
                                 });
            }

            return tasks;
        }
    }

    #endregion
}