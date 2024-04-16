namespace Samples.ToDo.UI;

#region << Using >>

using Extensions;
using Microsoft.JSInterop;

#endregion

public static class AppJsExt
{
    #region Constants

    private const string AppName = "ToDoSample";

    #endregion

    public static async Task InitDragulaAsync(this IJSRuntime js, object refObj, string[] statusesIds, string callbackName)
    {
        await js.InvokeVoidAsync($"{AppName}.Dragula.init", refObj, statusesIds, callbackName);
    }

    public static async Task CloseModalAsync(this IJSRuntime js, string modalId)
    {
        await js.InvokeVoidAsync($"{AppName}.Modal.close", modalId);
    }

    #region LocalStorage

    static async Task<string> getLocalStorageValueAsync(this IJSRuntime js, string key)
    {
        return await js.InvokeAsync<string>($"{AppName}.LocalStorage.get", key);
    }

    static async Task setLocalStorageValue(this IJSRuntime js, string key, string value)
    {
        await js.InvokeVoidAsync($"{AppName}.LocalStorage.set", key, value);
    }

    public static async Task SetLocalStorageAsync<T>(this IJSRuntime js, LocalStorage.Key key, T value)
    {
        var jsonValue = value?.ToJsonString() ?? string.Empty;

        await js.setLocalStorageValue(key.ToString(), jsonValue);

        LocalStorage.AddOrUpdate(key, jsonValue);
    }

    public static async Task FetchLocalStorageValuesAsync(this IJSRuntime js)
    {
        foreach (var key in Enum.GetValues<LocalStorage.Key>())
        {
            var value = await js.getLocalStorageValueAsync(key.ToString());

            LocalStorage.AddOrUpdate(key, value);
        }
    }

    #endregion
}