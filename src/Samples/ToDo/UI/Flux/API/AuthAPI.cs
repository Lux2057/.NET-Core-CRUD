namespace Samples.ToDo.UI;

#region << Using >>

using Samples.ToDo.Shared;

#endregion

public class AuthAPI : HttpBase
{
    #region Constructors

    public AuthAPI(HttpClient http) : base(http) { }

    #endregion

    public async Task<AuthResultDto> SignInAsync(AuthRequest request,
                                                 Action<ValidationFailureResult> validationFailCallback = null,
                                                 CancellationToken cancellationToken = default)
    {
        var httpResponse = await this.Http.SendApiRequestAsync(httpMethod: HttpMethodType.POST,
                                                               uri: ApiRoutes.SignIn,
                                                               accessToken: null,
                                                               content: request,
                                                               cancellationToken: cancellationToken);

        var result = await httpResponse.ToApiResponseOrDefaultAsync<AuthResultDto>(validationFailCallback);

        return result ?? new AuthResultDto { Success = false };
    }

    public async Task<AuthResultDto> SignUpAsync(AuthRequest request,
                                                 Action<ValidationFailureResult> validationFailCallback = null,
                                                 CancellationToken cancellationToken = default)
    {
        var httpResponse = await this.Http.SendApiRequestAsync(httpMethod: HttpMethodType.POST,
                                                               uri: ApiRoutes.SignUp,
                                                               accessToken: null,
                                                               content: request,
                                                               cancellationToken: cancellationToken);

        var result = await httpResponse.ToApiResponseOrDefaultAsync<AuthResultDto>(validationFailCallback);

        return result ?? new AuthResultDto { Success = false };
    }

    public async Task<AuthResultDto> RefreshTokenAsync(RefreshTokenRequestDto request,
                                                       Action<ValidationFailureResult> validationFailCallback = null,
                                                       CancellationToken cancellationToken = default)
    {
        var httpResponse = await this.Http.SendApiRequestAsync(httpMethod: HttpMethodType.POST,
                                                               uri: ApiRoutes.RefreshToken,
                                                               accessToken: null,
                                                               content: request,
                                                               cancellationToken: cancellationToken);

        var result = await httpResponse.ToApiResponseOrDefaultAsync<AuthResultDto>(validationFailCallback);

        return result ?? new AuthResultDto { Success = false };
    }
}