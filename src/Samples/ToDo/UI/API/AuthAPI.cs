namespace Samples.ToDo.UI;

#region << Using >>

using System.Net.Http.Json;
using Fluxor;
using Microsoft.Extensions.Localization;
using Samples.ToDo.Shared;
using Samples.ToDo.UI.Localization;

#endregion

public class AuthAPI : HttpBase
{
    #region Properties

    readonly IDispatcher dispatcher;

    readonly IStringLocalizer<Resource> localization;

    #endregion

    #region Constructors

    public AuthAPI(HttpClient http,
                   IStringLocalizer<Resource> localization,
                   IDispatcher dispatcher)
            : base(http)
    {
        this.localization = localization;
        this.dispatcher = dispatcher;
    }

    #endregion

    public async Task<AuthResultDto> SignInAsync(AuthRequest request, CancellationToken cancellationToken = default)
    {
        var httpResponse = await this.Http.PostAsJsonAsync(ApiRoutes.SignIn, request, cancellationToken);

        var result = await httpResponse.ToApiResponseOrDefaultAsync<AuthResultDto>(this.dispatcher, this.localization);

        return result ?? new AuthResultDto { Success = false };
    }

    public async Task<AuthResultDto> SignUpAsync(AuthRequest request, CancellationToken cancellationToken = default)
    {
        var httpResponse = await this.Http.PostAsJsonAsync(ApiRoutes.SignUp, request, cancellationToken);

        var result = await httpResponse.ToApiResponseOrDefaultAsync<AuthResultDto>(this.dispatcher, this.localization);

        return result ?? new AuthResultDto { Success = false };
    }

    public async Task<AuthResultDto> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default)
    {
        var httpResponse = await this.Http.PostAsJsonAsync(ApiRoutes.RefreshToken, request, cancellationToken);

        var result = await httpResponse.ToApiResponseOrDefaultAsync<AuthResultDto>(this.dispatcher, this.localization);

        return result ?? new AuthResultDto { Success = false };
    }
}