#region << Using >>

using Fluxor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Samples.UploadBigFile.UI;

#endregion

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddFluxor(o =>
                           {
                               o.ScanAssemblies(typeof(Program).Assembly, typeof(ReadToDoListsWf).Assembly);
                               o.UseReduxDevTools(rdt =>
                                                  {
                                                      rdt.Name = "Templates.Blazor.EF.UI";
                                                      rdt.EnableStackTrace();
                                                  });
                           });

builder.Services.AddLocalization();

await builder.Build().RunAsync();