namespace Samples.ToDo.UI;

#region << Using >>

using CRUD.Core;
using Fluxor;
using JetBrains.Annotations;

#endregion

[FeatureState]
public class ProjectsPageState : ILoadingStatus
{
    #region Properties

    public bool IsLoading { get; }

    public bool IsCreating { get; }

    public bool IsEmpty => Projects?.Items?.Any() != true;

    public PaginatedResponseDto<ProjectStatedDto> Projects { get; }

    #endregion

    #region Constructors

    [UsedImplicitly]
    ProjectsPageState()
    {
        IsLoading = false;
        IsCreating = false;
        Projects = new();
    }

    public ProjectsPageState(bool isLoading,
                         bool isCreating,
                         PaginatedResponseDto<ProjectStatedDto> projects)
    {
        IsLoading = isLoading;
        IsCreating = isCreating;
        Projects = projects;
    }

    #endregion
}