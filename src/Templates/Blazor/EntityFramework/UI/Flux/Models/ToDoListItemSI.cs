namespace Templates.Blazor.EF.UI;

#region << Using >>

using Templates.Blazor.EF.Shared;

#endregion

public class ToDoListItemSI : ToDoListItemDto, IUpdatingStatus, ICloneable
{
    #region Properties

    public bool IsUpdating { get; set; }

    #endregion

    #region Interface Implementations

    public object Clone()
    {
        return new ToDoListItemSI
               {
                       Id = Id,
                       IsUpdating = IsUpdating,
                       Status = Status,
                       ToDoListId = ToDoListId,
                       Description = Description
               };
    }

    #endregion
}