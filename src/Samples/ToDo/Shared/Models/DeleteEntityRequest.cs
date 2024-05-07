namespace Samples.ToDo.Shared;

public interface IDeleteEntityRequest
{
    #region Properties

    public int Id { get; }

    #endregion
}

public class DeleteEntityRequest : IDeleteEntityRequest
{
    #region Properties

    public int Id { get; set; }

    #endregion
}