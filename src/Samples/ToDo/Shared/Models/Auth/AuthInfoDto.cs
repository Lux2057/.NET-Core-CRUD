namespace Samples.ToDo.Shared;

public class AuthInfoDto
{
    #region Properties

    public DateTime AuthenticatedAt { get; }

    public UserDto User { get; set; }

    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }

    #endregion

    #region Constructors

    public AuthInfoDto()
    {
        AuthenticatedAt = DateTime.UtcNow;
    }

    #endregion
}