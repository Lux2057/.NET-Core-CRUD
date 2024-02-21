namespace Samples.ToDo.Shared;

public class AuthInfoDto
{
    #region Properties

    public UserDto User { get; set; }

    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }

    #endregion
}