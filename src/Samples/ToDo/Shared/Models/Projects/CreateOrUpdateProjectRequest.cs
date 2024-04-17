namespace Samples.ToDo.Shared;

public interface ICreateOrUpdateProjectRequest
{
    #region Properties

    public int? Id { get; }

    public string Name { get; }

    public string Description { get; }

    #endregion
}

public class CreateOrUpdateProjectRequest : ICreateOrUpdateProjectRequest
{
    #region Properties

    public int? Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    #endregion
}