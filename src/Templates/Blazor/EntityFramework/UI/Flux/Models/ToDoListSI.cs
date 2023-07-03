namespace Templates.Blazor.EF.UI;

#region << Using >>

using Templates.Blazor.EF.Shared;

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