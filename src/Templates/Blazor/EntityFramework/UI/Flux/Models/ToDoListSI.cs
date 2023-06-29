namespace Templates.Blazor.EF.UI;

#region << Using >>

using Templates.Blazor.EF.Shared;

#endregion

public class ToDoListSI : ToDoListDto, IUpdatingStatus
{
    #region Properties

    public bool IsUpdating { get; set; }

    #endregion
}