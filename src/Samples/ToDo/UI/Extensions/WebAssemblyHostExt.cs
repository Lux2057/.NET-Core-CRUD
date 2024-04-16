namespace Samples.ToDo.UI;

#region << Using >>

using System.Globalization;
using Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Samples.ToDo.Shared;

#endregion

public static class WebAssemblyHostExt
{
    public static async Task InitLocalStorageAsync(this WebAssemblyHost host)
    {
        var js = host.Services.GetRequiredService<IJSRuntime>();

        await js.FetchLocalStorageValuesAsync();

        await js.setLanguageAsync();
    }

    static async Task setLanguageAsync(this IJSRuntime js)
    {
        if (LocalStorage.GetOrDefault(LocalStorage.Key.Language).IsNullOrWhitespace())
            await js.SetLocalStorageAsync(LocalStorage.Key.Language, LocalizationConst.DefaultLanguage);

        var language = JsonConvert.DeserializeObject<string>(LocalStorage.GetOrDefault(LocalStorage.Key.Language))!;

        var culture = new CultureInfo(language);

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
}