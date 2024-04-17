namespace Samples.ToDo.Shared;

public class TaskDto
{
    #region Properties

    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public TaskStatus Status { get; set; }

    #endregion
}