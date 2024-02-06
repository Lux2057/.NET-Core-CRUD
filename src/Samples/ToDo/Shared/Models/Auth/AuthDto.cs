namespace Samples.ToDo.Shared.Auth;

public abstract class AuthDto
{
    #region Nested Classes

    public class Request
    {
        #region Properties

        public string UserName { get; set; }

        public string Password { get; set; }

        #endregion
    }

    public class Result
    {
        #region Properties

        public bool Success { get; set; }

        public UserDto User { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public string Message { get; set; }

        #endregion
    }

    #endregion
}