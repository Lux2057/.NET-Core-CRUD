namespace Samples.ToDo.UI;

#region << Using >>

using CRUD.Core;
using Fluxor;
using JetBrains.Annotations;

#endregion

[FeatureState]
public class TasksState : ILoadingStatus
{
    #region Properties

    public bool IsLoading { get; }

    public bool IsCreating { get; }

    public int ProjectId { get; }

    public PaginatedResponseDto<TaskStateDto> Tasks { get; }

    #endregion

    #region Constructors

    [UsedImplicitly]
    TasksState()
    {
        IsLoading = false;
        IsCreating = false;
        ProjectId = 0;
        Tasks = new();
    }

    public TasksState(bool isLoading,
                      bool isCreating,
                      int projectId,
                      PaginatedResponseDto<TaskStateDto> tasks)
    {
        IsLoading = isLoading;
        IsCreating = isCreating;
        ProjectId = projectId;
        Tasks = tasks;
    }

    #endregion
}