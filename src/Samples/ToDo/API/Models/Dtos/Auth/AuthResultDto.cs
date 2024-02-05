namespace Samples.ToDo.API;

public class AuthResultDto
{
    #region Properties

    public bool Success { get; set; }

    public UserDto User { get; set; }

    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }

    public string Message { get; set; }

    #endregion
}