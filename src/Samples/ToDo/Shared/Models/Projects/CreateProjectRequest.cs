namespace Samples.ToDo.Shared;

public class CreateProjectRequest
{
    #region Properties

    public string Name { get; set; }

    public string Description { get; set; }

    public int[] TagsIds { get; set; }

    #endregion
}