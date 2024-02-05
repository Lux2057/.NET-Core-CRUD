namespace Samples.ToDo.API;

public class RefreshTokenRequest
{
    #region Properties

    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }

    #endregion
}