namespace Samples.ToDo.UI;

#region << Using >>

#region << Using >>

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Extensions;
using Fluxor;
using Newtonsoft.Json;
using Samples.ToDo.Shared;

#endregion

#endregion

public static class ApiExt
{
    public static string ToApiParams<T>(this IEnumerable<T> enumerable, string paramName)
    {
        var array = enumerable.ToArrayOrEmpty();

        if (paramName.IsNullOrWhitespace() || !array.Any())
            return string.Empty;

        return array.Select(r => $"{paramName}={r}").ToJoinedString("&");
    }

    public static async Task<TResponse> GetApiResponseOrDefaultAsync<TResponse>(this HttpClient http,
                                                                                IDispatcher dispatcher,
                                                                                HttpMethodType httpMethod,
                                                                                string uri,
                                                                                string validationKey = null,
                                                                                string accessToken = null,
                                                                                object content = null,
                                                                                CancellationToken cancellationToken = default)
    {
        dispatcher.Dispatch(new SetValidationStateWf.Init(validationKey, null));

        var httpResponse = await http.sendApiRequestAsync(httpMethod: httpMethod,
                                                          uri: uri,
                                                          accessToken: accessToken,
                                                          content: content,
                                                          cancellationToken: cancellationToken);

        return await httpResponse.ToApiResponseOrDefaultAsync<TResponse>(validationFailCallback: result => dispatcher.Dispatch(new SetValidationStateWf.Init(validationKey, result)));
    }

    static async Task<HttpResponseMessage> sendApiRequestAsync(this HttpClient http,
                                                               HttpMethodType httpMethod,
                                                               string uri,
                                                               string accessToken = null,
                                                               object content = null,
                                                               CancellationToken cancellationToken = default)
    {
        var request = httpMethod switch
        {
                HttpMethodType.GET => new HttpRequestMessage(HttpMethod.Get, uri),
                HttpMethodType.POST => new HttpRequestMessage(HttpMethod.Post, uri),
                HttpMethodType.PUT => new HttpRequestMessage(HttpMethod.Put, uri),
                HttpMethodType.DELETE => new HttpRequestMessage(HttpMethod.Delete, uri),
                _ => throw new NotImplementedException($"{nameof(HttpMethodType)}: {httpMethod}")
        };

        if (!accessToken.IsNullOrWhitespace())
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var acceptLanguage = LocalStorage.GetOrDefault<string>(LocalStorage.Key.Language);
        if (!acceptLanguage.IsNullOrWhitespace())
            request.Headers.Add("Accept-Language", acceptLanguage);

        if (httpMethod != HttpMethodType.GET && content != null)
            request.Content = new StringContent(JsonConvert.SerializeObject(content, new JsonSerializerSettings
                                                                                     {
                                                                                             ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                                                                     }), Encoding.UTF8, "application/json");

        return await http.SendAsync(request, cancellationToken);
    }

    static async Task<TResponse> ToApiResponseOrDefaultAsync<TResponse>(this HttpResponseMessage response, Action<ValidationFailureResult> validationFailCallback = null)
    {
        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                return await response.Content.ReadFromJsonAsync<TResponse>();

            case HttpStatusCode.BadRequest:
                validationFailCallback?.Invoke(await response.Content.ReadFromJsonAsync<ValidationFailureResult>());
                return default;

            case HttpStatusCode.Forbidden:
                throw new UnauthorizedAccessException();

            default:
                throw new HttpRequestException(null, null, response.StatusCode);
        }
    }
}