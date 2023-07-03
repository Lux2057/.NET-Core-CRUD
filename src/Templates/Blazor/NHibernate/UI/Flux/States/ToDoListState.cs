namespace Templates.Blazor.NH.UI;

#region << Using >>

using CRUD.Core;
using Fluxor;
using JetBrains.Annotations;

#endregion

[FeatureState]
public class ToDoListState : ILoadingStatus
{
    #region Properties

    public int Id { get; }

    public bool IsLoading { get; }

    public bool IsCreating { get; }

    public bool IsEmpty => Items?.Items?.Any() != true;

    public PaginatedResponseDto<ToDoListItemSI> Items { get; }

    #endregion

    #region Constructors

    [UsedImplicitly]
    ToDoListState()
    {
        IsLoading = false;
        IsCreating = false;
        Items = new PaginatedResponseDto<ToDoListItemSI>
                {
                        Items = Array.Empty<ToDoListItemSI>(),
                        PagingInfo = new PagingInfoDto
                                     {
                                             CurrentPage = 1,
                                             PageSize = 1,
                                             TotalItemsCount = 0,
                                             TotalPages = 1
                                     }
                };
    }

    public ToDoListState(int id, bool isLoading, bool isCreating, PaginatedResponseDto<ToDoListItemSI> items)
    {
        Id = id;
        IsLoading = isLoading;
        IsCreating = isCreating;
        Items = items;
    }

    #endregion
}