namespace Samples.ToDo.Shared;

public interface ICreateOrUpdateTaskRequest
{
    #region Properties

    public int? Id { get; }

    public string Name { get; }

    public int ProjectId { get; }

    public int StatusId { get; }

    public string Description { get; }

    public DateTime? DueDate { get; }

    public int[] TagsIds { get; }

    #endregion
}

public class CreateOrUpdateTaskRequest : ICreateOrUpdateProjectRequest
{
    #region Properties

    public int? Id { get; }

    public string Name { get; set; }

    public int ProjectId { get; set; }

    public int StatusId { get; set; }

    public string Description { get; set; }

    public DateTime? DueDate { get; set; }

    public int[] TagsIds { get; set; }

    #endregion
}