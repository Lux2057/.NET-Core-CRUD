#region << Using >>

using CRUD.Core;
using CRUD.CQRS;
using CRUD.DAL.EntityFramework;
using CRUD.Logging.Common;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;
using Templates.Blazor.EF.API;
using Templates.Blazor.EF.Shared;

#endregion

var builder = WebApplication.CreateBuilder(args);

#region Services config

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen(c =>
                               {
                                   c.EnableAnnotations();
                                   c.OrderActionsBy(r => r.GroupName);
                               });

var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Services.AddEntityFrameworkDAL<ApiDbContext>(dbContextOptions: options =>
                                                                       {
                                                                           options.UseNpgsql(defaultConnectionString);
                                                                           options.UseLazyLoadingProxies();
                                                                       });

builder.Services.AddCQRS(mediatorAssemblies: new[]
                                             {
                                                     typeof(AddLogCommand).Assembly,
                                                     typeof(ReadEntitiesQuery<,,>).Assembly,
                                                     typeof(Program).Assembly
                                             },
                         validatorAssemblies: new[]
                                              {
                                                      typeof(AddLogCommand).Assembly,
                                                      typeof(ReadEntitiesQuery<,,>).Assembly,
                                                      typeof(Program).Assembly
                                              },
                         automapperAssemblies: new[]
                                               {
                                                       typeof(LogEntity).Assembly,
                                                       typeof(Program).Assembly
                                               });

builder.Services.AddEntityRead<LogEntity, int, LogDto>();
builder.Services.AddEntityRead<ToDoListEntity, int, ToDoListDto>();
builder.Services.AddEntityCRUD<ToDoListItemEntity, int, ToDoListItemDto>();

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

app.UseMiddleware<ExceptionsHandlerMiddleware>();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

#endregion

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    using (var context = serviceScope.ServiceProvider.GetService<ApiDbContext>()!)
    {
        context.Database.EnsureCreated();
        context.Database.Migrate();
    }
}

app.Run();