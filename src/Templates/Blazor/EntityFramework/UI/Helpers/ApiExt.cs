namespace Templates.Blazor.EF.UI;

#region << Using >>

using System.Net.Http.Json;
using CRUD.Core;
using CRUD.Extensions;
using Templates.Blazor.EF.Shared;

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

    public static async Task<PaginatedResponseDto<T>> ReadToDoListsAsync<T>(this HttpClient http, int page) where T : ToDoListDto
    {
        var uri = $"{ApiRoutes.ReadToDoLists}?{nameof(ApiRoutes.Params.page)}={page}";

        return await http.GetFromJsonAsync<PaginatedResponseDto<T>>(uri);
    }

    public static async Task CreateOrUpdateToDoListAsync(this HttpClient http, ToDoListDto dto)
    {
        var uri = ApiRoutes.CreateOrUpdateToDoLists;

        await http.PostAsJsonAsync(uri, dto);
    }

    public static async Task DeleteToDoListAsync(this HttpClient http, int id)
    {
        var uri = $"{ApiRoutes.DeleteToDoLists}?{new[] { id }.ToApiParams(nameof(ApiRoutes.Params.ids))}";

        await http.DeleteAsync(uri);
    }
}