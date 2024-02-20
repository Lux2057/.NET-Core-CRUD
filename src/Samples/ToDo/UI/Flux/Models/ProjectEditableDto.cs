namespace Samples.ToDo.UI;

#region << Using >>

using Samples.ToDo.Shared;

#endregion

public class ProjectEditableDto : ProjectDto, IUpdatingStatus, ICloneable
{
    #region Properties

    public bool IsUpdating { get; set; }

    #endregion

    #region Interface Implementations

    public object Clone()
    {
        return new ProjectEditableDto
               {
                       Id = Id,
                       Name = Name,
                       IsUpdating = IsUpdating,
                       Description = Description,
                       Tags = (TagDto[])Tags?.Clone() ?? Array.Empty<TagDto>()
               };
    }

    #endregion
}