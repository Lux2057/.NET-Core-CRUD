namespace Templates.Blazor.EF.UI;

#region << Using >>

using CRUD.Core;
using Fluxor;
using JetBrains.Annotations;

#endregion

[FeatureState]
public class ToDoListsState : ILoadingStatus
{
    #region Properties

    public bool IsLoading { get; }

    public bool IsCreating { get; }

    public bool IsEmpty => ToDoLists?.Items?.Any() != true;

    public PaginatedResponseDto<ToDoListSI> ToDoLists { get; }

    #endregion

    #region Constructors

    [UsedImplicitly]
    ToDoListsState()
    {
        IsLoading = false;
        IsCreating = false;
        ToDoLists = new PaginatedResponseDto<ToDoListSI>
                    {
                            Items = Array.Empty<ToDoListSI>(),
                            PagingInfo = new PagingInfoDto
                                         {
                                                 CurrentPage = 1,
                                                 PageSize = 1,
                                                 TotalItemsCount = 0,
                                                 TotalPages = 1
                                         }
                    };
    }

    public ToDoListsState(bool isLoading, bool isCreating, PaginatedResponseDto<ToDoListSI> toDoLists)
    {
        IsLoading = isLoading;
        IsCreating = isCreating;
        ToDoLists = toDoLists;
    }

    #endregion
}