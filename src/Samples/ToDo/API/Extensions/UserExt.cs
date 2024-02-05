namespace Samples.ToDo.API;

#region << Using >>

using System.Security.Claims;
using Extensions;

#endregion

public static class UserExt
{
    public static Claim[] GetClaims(this UserEntity user)
    {
        return new[]
               {
                       new Claim(nameof(UserDto.Id), user.Id.ToString()),
                       new Claim(nameof(UserDto.UserName), user.UserName)
               };
    }

    public static int GetUserIdOrDefault(this ClaimsPrincipal principal, int defaultValue = 0)
    {
        var claimsArray = principal?.Claims.ToArrayOrEmpty() ?? Array.Empty<Claim>();

        if (claimsArray.Length == 0)
            return defaultValue;

        var id = claimsArray.SingleOrDefault(r => r.Type == nameof(UserDto.Id));

        return id == null ? defaultValue : Convert.ToInt32(id.Value);
    }
}