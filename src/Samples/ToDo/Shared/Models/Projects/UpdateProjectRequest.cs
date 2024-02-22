namespace Samples.ToDo.Shared;

public interface IUpdateProjectRequest : ICreateProjectRequest
{
    #region Properties

    public int Id { get; set; }

    #endregion
}

public class UpdateProjectRequest : IUpdateProjectRequest
{
    #region Properties

    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int[] TagsIds { get; set; }

    #endregion
}