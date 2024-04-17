namespace Samples.ToDo.API;

#region << Using >>

using AutoMapper.QueryableExtensions;
using CRUD.CQRS;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Samples.ToDo.Shared;

#endregion

public class GetTasksQuery : QueryBase<TaskDto[]>
{
    #region Properties

    public int UserId { get; }

    public int ProjectId { get; }

    public string SearchTerm { get; }

    #endregion

    #region Constructors

    public GetTasksQuery(int userId,
                         int projectId,
                         string searchTerm)
    {
        UserId = userId;
        ProjectId = projectId;
        SearchTerm = searchTerm?.Trim();
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
            var tasksSpec = new IsDeletedProp.FindBy.EqualTo<TaskEntity>(false) &&
                            new UserIdProp.FindBy.EqualTo<TaskEntity>(request.UserId) &&
                            new ProjectIdProp.FindBy.EqualTo<TaskEntity>(request.ProjectId) &&
                            (new NameProp.FindBy.ContainedTerm<TaskEntity>(request.SearchTerm) ||
                             new DescriptionProp.FindBy.ContainedTerm<TaskEntity>(request.SearchTerm));

            var tasks = await Repository.Read(tasksSpec)
                                        .ProjectTo<TaskDto>(Mapper.ConfigurationProvider)
                                        .ToArrayAsync(cancellationToken);

            return tasks.Length == 0 ? Array.Empty<TaskDto>() : tasks;
        }
    }

    #endregion
}