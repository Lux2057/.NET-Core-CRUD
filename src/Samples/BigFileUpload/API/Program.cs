#region << Using >>

using CRUD.WebAPI;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Swashbuckle.AspNetCore.SwaggerUI;

#endregion

var builder = WebApplication.CreateBuilder(args);

#region Services config

builder.Services.Configure<IISServerOptions>(options =>
                                             {
                                                 options.MaxRequestBodySize = int.MaxValue;
                                             });

builder.Services.Configure<KestrelServerOptions>(options =>
                                                 {
                                                     options.Limits.MaxRequestBodySize = int.MaxValue;
                                                 });

builder.Services.Configure<FormOptions>(options =>
                                        {
                                            options.ValueLengthLimit = int.MaxValue;
                                            options.MultipartBodyLengthLimit = int.MaxValue;
                                            options.MultipartHeadersLengthLimit = int.MaxValue;
                                        });

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen(c =>
                               {
                                   c.EnableAnnotations();
                                   c.OrderActionsBy(r => r.GroupName);
                               });

builder.Services.AddChunksStorage(TimeSpan.FromMinutes(5));

#endregion

var app = builder.Build();

#region App config

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(c =>
                 {
                     c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "API V1");
                     c.RoutePrefix = "swagger";
                     c.DocExpansion(DocExpansion.None);
                 });

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

#endregion

app.Run();