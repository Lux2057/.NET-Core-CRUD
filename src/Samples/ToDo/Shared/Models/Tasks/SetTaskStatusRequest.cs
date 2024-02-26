namespace Samples.ToDo.Shared;

public interface ISetTaskStatusRequest
{
    #region Properties

    public int Id { get; }

    public int StatusId { get; }

    #endregion
}

public class SetTaskStatusRequest : ISetTaskStatusRequest
{
    #region Properties

    public int Id { get; set; }

    public int StatusId { get; set; }

    #endregion
}