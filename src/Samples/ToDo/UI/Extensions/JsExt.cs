﻿namespace Samples.ToDo.UI;

#region << Using >>

using Microsoft.JSInterop;
using System.Linq.Expressions;

#endregion

public static class JsExt
{
    public static async Task CloseModalAsync(this IJSRuntime js, string modalId)
    {
        await js.InvokeVoidAsync("closeModal", modalId);
    }

    public static async Task<string> GetBlazorLanguageAsync(this IJSRuntime js)
    {
        return await js.InvokeAsync<string>("blazorCulture.get");
    }

    public static async Task SetBlazorLanguageAsync(this IJSRuntime js, string language)
    {
        await js.InvokeVoidAsync("blazorCulture.set", language);
    }

    public static async Task InitDragulaAsync(this IJSRuntime js, object refObj, string[] statusesIds, string callbackName)
    {
        await js.InvokeVoidAsync("initDragula", refObj, statusesIds, callbackName);
    }
}