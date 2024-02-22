namespace Samples.ToDo.Shared;

public interface ICreateProjectRequest
{
    #region Properties

    public string Name { get; }

    public string Description { get; }

    public int[] TagsIds { get; }

    #endregion
}

public class CreateProjectRequest : ICreateProjectRequest
{
    #region Properties

    public string Name { get; set; }

    public string Description { get; set; }

    public int[] TagsIds { get; set; }

    #endregion
}