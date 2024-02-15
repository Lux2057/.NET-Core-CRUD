namespace Samples.ToDo.UI;

#region << Using >>

using System.Net.Http.Json;
using Samples.ToDo.Shared;

#endregion

public class AuthAPI : HttpBase
{
    #region Constructors

    public AuthAPI(HttpClient http) : base(http) { }

    #endregion

    public async Task<AuthResultDto> SignInAsync(AuthRequest request, CancellationToken cancellationToken = default)
    {
        var httpResponse = await this.Http.PostAsJsonAsync(ApiRoutes.SignIn, request, cancellationToken);

        return await httpResponse.Content.ReadFromJsonAsync<AuthResultDto>();
    }

    public async Task<AuthResultDto> SignUpAsync(AuthRequest request, CancellationToken cancellationToken = default)
    {
        var httpResponse = await this.Http.PostAsJsonAsync(ApiRoutes.SignUp, request, cancellationToken);

        return await httpResponse.Content.ReadFromJsonAsync<AuthResultDto>();
    }

    public async Task<AuthResultDto> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default)
    {
        var httpResponse = await this.Http.PostAsJsonAsync(ApiRoutes.RefreshToken, request, cancellationToken);

        return await httpResponse.Content.ReadFromJsonAsync<AuthResultDto>();
    }
}