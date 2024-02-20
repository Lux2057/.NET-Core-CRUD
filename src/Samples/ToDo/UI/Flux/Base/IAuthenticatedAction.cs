namespace Samples.ToDo.UI;

public interface IAuthenticatedAction
{
    #region Properties

    public string AccessToken { get; set; }

    #endregion
}