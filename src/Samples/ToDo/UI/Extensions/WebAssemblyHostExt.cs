namespace Samples.ToDo.UI;

#region << Using >>

using System.Globalization;
using Fluxor;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;

#endregion

public static class WebAssemblyHostExt
{
    public static async Task InitLanguageAsync(this WebAssemblyHost host, string defaultLanguage)
    {
        CultureInfo culture;
        var js = host.Services.GetRequiredService<IJSRuntime>();
        var result = await js.GetBlazorLanguageAsync();

        if (result != null)
        {
            culture = new CultureInfo(result);
        }
        else
        {
            culture = new CultureInfo(defaultLanguage);
            await js.SetBlazorLanguageAsync(defaultLanguage);
        }

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
}