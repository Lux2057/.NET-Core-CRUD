namespace Samples.ToDo.Shared;

public class CreateTaskRequest
{
    #region Properties

    public string Name { get; set; }

    public int ProjectId { get; set; }

    public string Description { get; set; }

    public DateTime? DueDate { get; set; }

    public int[] TagsIds { get; set; }

    #endregion
}