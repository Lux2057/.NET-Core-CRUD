namespace Samples.ToDo.API;

#region << Using >>

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CRUD.CQRS;
using Extensions;
using JetBrains.Annotations;
using Microsoft.IdentityModel.Tokens;
using Samples.ToDo.Shared.Auth;

#endregion

public class GetAccessTokenQuery : QueryBase<string>
{
    #region Properties

    public Claim[] Claims { get; }

    #endregion

    #region Constructors

    public GetAccessTokenQuery(IEnumerable<Claim> claims)
    {
        Claims = claims.ToArrayOrEmpty();
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Handler : QueryHandlerBase<GetAccessTokenQuery, string>
    {
        #region Properties

        private readonly JwtAuthSettings jwtAuthSettings;

        #endregion

        #region Constructors

        public Handler(IServiceProvider serviceProvider,
                       JwtAuthSettings jwtAuthSettings) : base(serviceProvider)
        {
            this.jwtAuthSettings = jwtAuthSettings;
        }

        #endregion

        protected override Task<string> Execute(GetAccessTokenQuery request, CancellationToken cancellationToken)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.jwtAuthSettings.Secret));
            var credentials = new SigningCredentials(key, this.jwtAuthSettings.SecurityAlgorithm);

            var token = new JwtSecurityToken(issuer: this.jwtAuthSettings.Issuer,
                                             audience: this.jwtAuthSettings.Audience,
                                             notBefore: DateTime.Now,
                                             claims: request.Claims,
                                             expires: DateTime.Now.AddMinutes(this.jwtAuthSettings.AccessTokenExpirationInMinutes),
                                             signingCredentials: credentials);

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }

    #endregion
}