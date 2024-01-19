namespace Samples.UploadBigFile.UI;

#region << Using >>

using System.Net.Http.Json;
using CRUD.Core;
using Extensions;
using Samples.UploadBigFile.Shared;

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