namespace Samples.ToDo.Shared;

public class EditTaskRequest
{
    #region Properties

    public int Id { get; set; }

    public string Name { get; set; }

    public int StatusId { get; set; }

    public int ProjectId { get; set; }

    public string Description { get; set; }

    public DateTime? DueDate { get; set; }

    public int[] TagsIds { get; set; }

    #endregion
}