namespace Samples.ToDo.UI;

#region << Using >>

using CRUD.Core;
using Fluxor;
using JetBrains.Annotations;

#endregion

[FeatureState]
public class ProjectPageState : ILoadingStatus
{
    #region Properties

    public bool IsLoading { get; }

    public bool IsCreating { get; }

    public int ProjectId { get; }

    public PaginatedResponseDto<TaskStateDto> Tasks { get; }

    public StatusStateDto[] Statuses { get; }

    #endregion

    #region Constructors

    [UsedImplicitly]
    ProjectPageState()
    {
        Statuses = Array.Empty<StatusStateDto>();
        IsLoading = false;
        IsCreating = false;
        ProjectId = 0;
        Tasks = new();
    }

    public ProjectPageState(bool isLoading,
                            bool isCreating,
                            int projectId,
                            PaginatedResponseDto<TaskStateDto> tasks,
                            StatusStateDto[] statuses)
    {
        IsLoading = isLoading;
        IsCreating = isCreating;
        ProjectId = projectId;
        Tasks = tasks;
        Statuses = statuses;
    }

    #endregion
}