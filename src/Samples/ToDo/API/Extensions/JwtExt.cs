namespace Samples.ToDo.API;

#region << Using >>

using System.Security.Claims;
using System.Text;
using Extensions;
using Microsoft.IdentityModel.Tokens;
using Samples.ToDo.Shared;

#endregion

public static class JwtExt
{
    public static Claim[] GetClaims(this UserEntity user)
    {
        return new[]
               {
                       new Claim(nameof(UserDto.Id), user.Id.ToString()),
                       new Claim(nameof(UserDto.UserName), user.UserName)
               };
    }

    public static UserDto ToUserDto(this ClaimsPrincipal principal)
    {
        var claimsArray = principal?.Claims.ToArrayOrEmpty() ?? Array.Empty<Claim>();

        if (claimsArray.Length == 0)
            return null;

        var id = claimsArray.SingleOrDefault(r => r.Type == nameof(UserDto.Id));
        var userName = claimsArray.SingleOrDefault(r => r.Type == nameof(UserDto.UserName));

        return id == null ?
                       null :
                       new UserDto
                       {
                               Id = Convert.ToInt32(id.Value),
                               UserName = userName?.Value
                       };
    }

    public static SecurityKey GetSecurityKey(this string secret)
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
    }
}