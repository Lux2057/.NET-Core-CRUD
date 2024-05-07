namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using JetBrains.Annotations;

#endregion

[FeatureState]
public class TasksPageState : ILoadingStatus
{
    #region Properties

    public bool IsLoading { get; }

    public bool IsCreating { get; }

    public int ProjectId { get; }

    public TaskStateDto[] Tasks { get; }

    #endregion

    #region Constructors

    [UsedImplicitly]
    TasksPageState()
    {
        IsLoading = false;
        IsCreating = false;
        ProjectId = 0;
        Tasks = Array.Empty<TaskStateDto>();
    }

    public TasksPageState(bool isLoading,
                          bool isCreating,
                          int projectId,
                          TaskStateDto[] tasks)
    {
        IsLoading = isLoading;
        IsCreating = isCreating;
        ProjectId = projectId;
        Tasks = tasks;
    }

    #endregion
}