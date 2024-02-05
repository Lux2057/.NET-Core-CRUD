namespace Samples.ToDo.API;

#region << Using >>

using Microsoft.IdentityModel.Tokens;

#endregion

public class JwtAuthSettings
{
    #region Properties

    public string Secret { get; init; }

    public string Issuer { get; init; }

    public string Audience { get; init; }

    public int AccessTokenExpirationInMinutes { get; init; }

    public int RefreshTokenExpirationInMinutes { get; init; }

    public string SecurityAlgorithm { get; init; } = SecurityAlgorithms.HmacSha256;

    #endregion
}