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

    public PaginatedResponseDto<ToDoListSI> ToDoLists { get; }

    #endregion

    #region Constructors

    [UsedImplicitly]
    ToDoListsState()
    {
        IsLoading = false;
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

    public ToDoListsState(bool isLoading, PaginatedResponseDto<ToDoListSI> toDoLists)
    {
        IsLoading = isLoading;
        ToDoLists = toDoLists;
    }

    #endregion
}