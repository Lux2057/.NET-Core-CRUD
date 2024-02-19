namespace Samples.ToDo.UI;

#region << Using >>

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using CRUD.Core;
using Extensions;
using Fluxor;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Samples.ToDo.Shared;
using Samples.ToDo.UI.Localization;

#endregion

public static class ApiExt
{
    public static async Task<HttpResponseMessage> SendAuthenticatedRequestAsync(this HttpClient http,
                                                                                HttpMethodType httpMethod,
                                                                                string uri,
                                                                                string accessToken,
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

    #region ToDoLists

    public static async Task<PaginatedResponseDto<T>> ReadToDoListsAsync<T>(this HttpClient http, int page) where T : ToDoListDto
    {
        var uri = $"{ApiRoutes.ReadToDoLists}?{nameof(ApiRoutes.Params.page)}={page}";

        return await http.GetFromJsonAsync<PaginatedResponseDto<T>>(uri);
    }

    public static async Task<HttpResponseMessage> CreateOrUpdateToDoListAsync(this HttpClient http, ToDoListDto dto)
    {
        const string uri = ApiRoutes.CreateOrUpdateToDoList;

        return await http.PostAsJsonAsync(uri, dto);
    }

    public static async Task<HttpResponseMessage> DeleteToDoListAsync(this HttpClient http, int id)
    {
        var uri = $"{ApiRoutes.DeleteToDoList}?{nameof(ApiRoutes.Params.id)}={id}";

        return await http.DeleteAsync(uri);
    }

    #endregion

    #region ToDoListItems

    public static async Task<PaginatedResponseDto<T>> ReadToDoListItemsAsync<T>(this HttpClient http, int toDoListId, int page) where T : ToDoListItemDto
    {
        var uri = $"{ApiRoutes.ReadToDoListItems}?{nameof(ApiRoutes.Params.toDoListId)}={toDoListId}&{nameof(ApiRoutes.Params.page)}={page}";

        return await http.GetFromJsonAsync<PaginatedResponseDto<T>>(uri);
    }

    public static async Task<HttpResponseMessage> CreateOrUpdateToDoListItemAsync(this HttpClient http, ToDoListItemDto dto)
    {
        const string uri = ApiRoutes.CreateOrUpdateToDoListItem;

        return await http.PostAsJsonAsync(uri, dto);
    }

    public static async Task<HttpResponseMessage> DeleteToDoListItemAsync(this HttpClient http, int id)
    {
        var uri = $"{ApiRoutes.DeleteToDoListItem}?{nameof(ApiRoutes.Params.id)}={id}";

        return await http.DeleteAsync(uri);
    }

    #endregion
}