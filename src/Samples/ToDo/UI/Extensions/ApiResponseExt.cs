namespace Samples.ToDo.UI;

#region << Using >>

using System.Net;
using System.Net.Http.Json;
using Fluxor;
using Microsoft.Extensions.Localization;
using Samples.ToDo.UI.Localization;

#endregion

public static class ApiResponseExt
{
    public static async Task<TResponse> ToApiResponseOrDefaultAsync<TResponse>(this HttpResponseMessage response,
                                                                               IDispatcher dispatcher,
                                                                               IStringLocalizer<Resource> localization)
    {
        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                return await response.Content.ReadFromJsonAsync<TResponse>();

            case HttpStatusCode.BadRequest:
                dispatcher.Dispatch(new SetValidationStateWf.Init(await response.Content.ReadFromJsonAsync<ValidationFailureResult>()));
                return default;

            default:
                dispatcher.Dispatch(new SetValidationStateWf.Init(new ValidationFailureResult($"{localization[Resource.Http_request_error]}: {response.StatusCode}", null)));
                return default;
        }
    }
}