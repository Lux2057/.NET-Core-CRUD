namespace Samples.ToDo.Shared;

public interface IAuthRequest
{
    #region Properties

    public string UserName { get; }

    public string Password { get; }

    #endregion
}

public class AuthRequest : IAuthRequest
{
    #region Properties

    public string UserName { get; set; }

    public string Password { get; set; }

    #endregion
}