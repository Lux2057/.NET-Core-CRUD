namespace Samples.ToDo.UI;

public interface IAuthRequiredAction
{
    #region Properties

    public string AccessToken { get; set; }

    #endregion
}