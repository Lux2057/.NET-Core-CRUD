namespace Templates.Blazor.NH.UI;

#region << Using >>

using Templates.Blazor.NH.Shared;

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