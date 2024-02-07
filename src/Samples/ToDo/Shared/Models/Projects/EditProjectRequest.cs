namespace Samples.ToDo.Shared;

public class EditProjectRequest
{
    #region Properties

    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int[] TagsIds { get; set; }

    #endregion
}