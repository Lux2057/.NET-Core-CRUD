namespace Samples.ToDo.UI;

#region << Using >>

using Samples.ToDo.Shared;

#endregion

public class ProjectStatedDto : ProjectDto, ICloneable
{
    #region Properties

    public bool IsUpdating { get; set; }

    public bool IsDeleting { get; set; }

    #endregion

    #region Interface Implementations

    public object Clone()
    {
        return new ProjectStatedDto
               {
                       Id = Id,
                       Name = Name,
                       Description = Description,
                       IsUpdating = IsUpdating
               };
    }

    #endregion
}