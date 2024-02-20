namespace Samples.ToDo.UI;

#region << Using >>

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Extensions;
using Newtonsoft.Json;
using Samples.ToDo.Shared;

#endregion

public static class ApiExt
{
    public static async Task<HttpResponseMessage> SendApiRequestAsync(this HttpClient http,
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

        if (httpMethod != HttpMethodType.GET && content != null)
            request.Content = new StringContent(JsonConvert.SerializeObject(content, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }), Encoding.UTF8, "application/json");

        return await http.SendAsync(request, cancellationToken);
    }

    public static string ToApiParams<T>(this IEnumerable<T> enumerable, string paramName)
    {
        var array = enumerable.ToArrayOrEmpty();

        if (paramName.IsNullOrWhitespace() || !array.Any())
            return string.Empty;

        return array.Select(r => $"{paramName}={r}").ToJoinedString("&");
    }

    public static async Task<TResponse> ToApiResponseOrDefaultAsync<TResponse>(this HttpResponseMessage response, Action<ValidationFailureResult> validationFailCallback = null)
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