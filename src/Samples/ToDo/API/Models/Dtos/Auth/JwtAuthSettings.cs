namespace Samples.ToDo.API;

#region << Using >>

using Microsoft.IdentityModel.Tokens;

#endregion

public class JwtAuthSettings
{
    #region Properties

    public string Secret { get; set; }

    public string Issuer { get; set; }

    public string Audience { get; set; }

    public int AccessTokenExpiration { get; set; }

    public int RefreshTokenExpirationInMinutes { get; set; }

    public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256;

    #endregion
}