namespace Samples.ToDo.Shared;

public interface ICreateOrUpdateStatusRequest
{
    #region Properties

    public int? Id { get; }

    public string Name { get; }

    #endregion
}

public class CreateOrUpdateStatusRequest : ICreateOrUpdateStatusRequest
{
    #region Properties

    public int? Id { get; set; }

    public string Name { get; set; }

    #endregion
}