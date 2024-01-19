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
                               o.ScanAssemblies(typeof(Program).Assembly, typeof(SetIsLoadingStatusWf).Assembly);
                               o.UseReduxDevTools(rdt =>
                                                  {
                                                      rdt.Name = "Samples.UploadBigFile.UI";
                                                      rdt.EnableStackTrace();
                                                  });
                           });

await builder.Build().RunAsync();