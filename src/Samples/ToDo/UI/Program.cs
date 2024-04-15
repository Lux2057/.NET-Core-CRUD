#region << Using >>

using Extensions;
using Fluxor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using Samples.ToDo.Shared;
using Samples.ToDo.UI;

#endregion

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddFluxor(o =>
                           {
                               o.ScanAssemblies(typeof(Program).Assembly, typeof(SignInWf).Assembly);
                               o.UseReduxDevTools(rdt =>
                                                  {
                                                      rdt.Name = "Samples.ToDo.UI";
                                                      rdt.EnableStackTrace();
                                                  });

                               o.AddMiddleware<AuthMiddleware>();
                           });

builder.Services.AddLocalization();

var host = builder.Build();

var js = host.Services.GetRequiredService<IJSRuntime>();
await LocalStorage.FetchBuiltInValuesAsync(js);

if (LocalStorage.GetBuiltInValueOrDefault(LocalStorage.Key.Language).IsNullOrWhitespace())
    await LocalStorage.SetAsync(js, LocalStorage.Key.Language, LocalizationConst.DefaultLanguage);

await host.InitLanguageAsync(LocalizationConst.DefaultLanguage);

await host.RunAsync();