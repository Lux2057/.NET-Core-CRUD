namespace Samples.ToDo.API;

#region << Using >>

using System.IdentityModel.Tokens.Jwt;
using System.Text;
using CRUD.CQRS;
using Extensions;
using JetBrains.Annotations;
using Microsoft.IdentityModel.Tokens;
using Samples.ToDo.Shared;

#endregion

public class GetTokenPrincipalQuery : QueryBase<TokenPrincipalDto>
{
    #region Properties

    public string Token { get; }

    #endregion

    #region Constructors

    public GetTokenPrincipalQuery(string token)
    {
        Token = token;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Handler : QueryHandlerBase<GetTokenPrincipalQuery, TokenPrincipalDto>
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

        protected override Task<TokenPrincipalDto> Execute(GetTokenPrincipalQuery request, CancellationToken cancellationToken)
        {
            if (request.Token.IsNullOrWhitespace())
                return Task.FromResult(new TokenPrincipalDto
                                       {
                                               Success = false,
                                               Message = ValidationMessagesConst.Token_is_empty
                                       });

            var tokenValidator = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.jwtAuthSettings.Secret));

            var parameters = new TokenValidationParameters
                             {
                                     ValidateAudience = false,
                                     ValidateIssuer = false,
                                     ValidateIssuerSigningKey = true,
                                     IssuerSigningKey = key,
                                     ValidateLifetime = false
                             };

            var result = new TokenPrincipalDto();
            try
            {
                var principal = tokenValidator.ValidateToken(request.Token, parameters, out var securityToken);

                if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(this.jwtAuthSettings.SecurityAlgorithm, StringComparison.InvariantCultureIgnoreCase))
                {
                    result.Success = false;
                    result.Message = ValidationMessagesConst.Token_is_invalid;
                }
                else
                {
                    result.Success = true;
                    result.Principal = principal;
                }
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = e.Message;
            }

            return Task.FromResult(result);
        }
    }

    #endregion
}