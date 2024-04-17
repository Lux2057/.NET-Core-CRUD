namespace Samples.ToDo.UI;

#region << Using >>

using Samples.ToDo.Shared;

#endregion

public class ProjectStateDto : ProjectDto, IUpdatingStatus, ICloneable
{
    #region Properties

    public bool IsUpdating { get; set; }

    #endregion

    #region Interface Implementations

    public object Clone()
    {
        return new ProjectStateDto
               {
                       Id = Id,
                       Name = Name,
                       Description = Description,
                       IsUpdating = IsUpdating
               };
    }

    #endregion
}