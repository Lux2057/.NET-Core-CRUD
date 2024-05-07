namespace Samples.ToDo.Shared;

public interface IRefreshRequest
{
    #region Properties

    public int UserId { get; }

    public string RefreshToken { get; }

    #endregion
}

public class RefreshTokenRequest : IRefreshRequest
{
    #region Properties

    public int UserId { get; set; }

    public string RefreshToken { get; set; }

    #endregion
}