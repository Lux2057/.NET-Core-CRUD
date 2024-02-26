namespace Samples.ToDo.UI;

#region << Using >>

using CRUD.Core;
using Fluxor;
using JetBrains.Annotations;

#endregion

[FeatureState]
public class ProjectsState : ILoadingStatus
{
    #region Properties

    public bool IsLoading { get; }

    public bool IsCreating { get; }

    public bool IsEmpty => Projects?.Items?.Any() != true;

    public PaginatedResponseDto<ProjectStateDto> Projects { get; }

    #endregion

    #region Constructors

    [UsedImplicitly]
    ProjectsState()
    {
        IsLoading = false;
        IsCreating = false;
        Projects = new PaginatedResponseDto<ProjectStateDto>
                    {
                            Items = Array.Empty<ProjectStateDto>(),
                            PagingInfo = new PagingInfoDto
                                         {
                                                 CurrentPage = 1,
                                                 PageSize = 1,
                                                 TotalItemsCount = 0,
                                                 TotalPages = 1
                                         }
                    };
    }

    public ProjectsState(bool isLoading, bool isCreating, PaginatedResponseDto<ProjectStateDto> projects)
    {
        IsLoading = isLoading;
        IsCreating = isCreating;
        Projects = projects;
    }

    #endregion
}