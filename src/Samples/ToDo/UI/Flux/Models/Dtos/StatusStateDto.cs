namespace Samples.ToDo.UI;

#region << Using >>

using Samples.ToDo.Shared;

#endregion

public class StatusStateDto : StatusDto, IUpdatingStatus, ICloneable
{
    #region Properties

    public bool IsUpdating { get; set; }

    #endregion

    #region Interface Implementations

    public object Clone()
    {
        return new StatusStateDto
               {
                       Id = Id,
                       Name = Name,
                       IsUpdating = IsUpdating
               };
    }

    #endregion
}