namespace Samples.ToDo.UI;

#region << Using >>

using Extensions;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Samples.ToDo.Shared;

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

    public static async Task<string> GetLocalStorageValueAsync(this IJSRuntime js, string key)
    {
        return await js.InvokeAsync<string>($"{AppName}.LocalStorage.get", key);
    }

    public static async Task SetLocalStorageValue(this IJSRuntime js, string key, string value)
    {
        await js.InvokeVoidAsync($"{AppName}.LocalStorage.set", key, value);
    }

    public static async Task<string> GetBlazorLanguageAsync(this IJSRuntime js)
    {
        return await js.InvokeAsync<string>("blazorCulture.get");
    }

    public static async Task SetBlazorLanguageAsync(this IJSRuntime js, string language)
    {
        await js.InvokeVoidAsync("blazorCulture.set", language);
    }
}