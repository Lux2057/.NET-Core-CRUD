namespace Samples.ToDo.API;

#region << Using >>

using System.Security.Claims;

#endregion

public class TokenPrincipalDto
{
    #region Properties

    public bool Success { get; set; }

    public string Message { get; set; }

    public ClaimsPrincipal Principal { get; set; }

    #endregion
}