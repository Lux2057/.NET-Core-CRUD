namespace Samples.ToDo.API;

#region << Using >>

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CRUD.CQRS;
using Extensions;
using JetBrains.Annotations;
using Microsoft.IdentityModel.Tokens;

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
            var dt = DateTime.Now;

            return Task.FromResult(new JwtSecurityTokenHandler()
                                           .CreateEncodedJwt(issuer: this.jwtAuthSettings.Issuer,
                                                             audience: this.jwtAuthSettings.Audience,
                                                             subject: new ClaimsIdentity(request.Claims),
                                                             issuedAt: dt,
                                                             notBefore: dt,
                                                             expires: dt.AddMinutes(this.jwtAuthSettings.AccessTokenExpirationInMinutes),
                                                             signingCredentials: new SigningCredentials(this.jwtAuthSettings.Secret.GetSecurityKey(),
                                                                                                        this.jwtAuthSettings.SecurityAlgorithm)));
        }
    }

    #endregion
}