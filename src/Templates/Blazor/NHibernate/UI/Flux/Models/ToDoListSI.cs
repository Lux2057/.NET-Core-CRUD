namespace Templates.Blazor.NH.UI;

#region << Using >>

using Templates.Blazor.NH.Shared;

#endregion

public class ToDoListSI : ToDoListDto, IUpdatingStatus, ICloneable
{
    #region Properties

    public bool IsUpdating { get; set; }

    #endregion

    #region Interface Implementations

    public object Clone()
    {
        return new ToDoListSI
               {
                       Id = Id,
                       Name = Name,
                       IsUpdating = IsUpdating,
                       CrDt = CrDt
               };
    }

    #endregion
}