namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using Samples.ToDo.Shared;

#endregion

public class AuthAPI : HttpBase
{
    #region Properties

    readonly IDispatcher dispatcher;

    #endregion

    #region Constructors

    public AuthAPI(HttpClient http, IDispatcher dispatcher) : base(http)
    {
        this.dispatcher = dispatcher;
    }

    #endregion

    public async Task<AuthInfoDto> SignInAsync(AuthRequest request,
                                                 string validationKey,
                                                 CancellationToken cancellationToken = default)
    {
        return await this.Http.GetApiResponseOrDefaultAsync<AuthInfoDto>(dispatcher: this.dispatcher,
                                                                           validationKey: validationKey,
                                                                           httpMethod: HttpMethodType.POST,
                                                                           uri: ApiRoutes.SignIn,
                                                                           accessToken: null,
                                                                           content: request,
                                                                           cancellationToken: cancellationToken);
    }

    public async Task<AuthInfoDto> SignUpAsync(AuthRequest request,
                                                 string validationKey,
                                                 CancellationToken cancellationToken = default)
    {
        return await this.Http.GetApiResponseOrDefaultAsync<AuthInfoDto>(dispatcher: this.dispatcher,
                                                                           validationKey: validationKey,
                                                                           httpMethod: HttpMethodType.POST,
                                                                           uri: ApiRoutes.SignUp,
                                                                           accessToken: null,
                                                                           content: request,
                                                                           cancellationToken: cancellationToken);
    }

    public async Task<AuthInfoDto> RefreshTokenAsync(RefreshTokenRequestDto request,
                                                       string validationKey,
                                                       CancellationToken cancellationToken = default)
    {
        return await this.Http.GetApiResponseOrDefaultAsync<AuthInfoDto>(dispatcher: this.dispatcher,
                                                                           validationKey: validationKey,
                                                                           httpMethod: HttpMethodType.POST,
                                                                           uri: ApiRoutes.RefreshToken,
                                                                           accessToken: null,
                                                                           content: request,
                                                                           cancellationToken: cancellationToken);
    }
}