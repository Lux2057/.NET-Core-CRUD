namespace Samples.ToDo.Shared;

public interface ISetTaskStatusRequest
{
    #region Properties

    public int Id { get; }

    public TaskStatus Status { get; }

    #endregion
}

public class SetTaskStatusRequest : ISetTaskStatusRequest
{
    #region Properties

    public int Id { get; set; }

    public TaskStatus Status { get; set; }

    #endregion
}