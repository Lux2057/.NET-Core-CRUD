namespace Samples.ToDo.Shared;

public interface ICreateOrUpdateTaskRequest
{
    #region Properties

    public int? Id { get; }

    public string Name { get; }

    public int ProjectId { get; }

    public TaskStatus Status { get; }

    public string Description { get; }

    #endregion
}

public class CreateOrUpdateTaskRequest : ICreateOrUpdateTaskRequest
{
    #region Properties

    public int? Id { get; }

    public string Name { get; set; }

    public int ProjectId { get; set; }

    public TaskStatus Status { get; set; }

    public string Description { get; set; }

    #endregion
}