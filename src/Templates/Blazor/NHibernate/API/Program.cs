#region << Using >>

using CRUD.Core;
using CRUD.CQRS;
using CRUD.DAL.NHibernate;
using CRUD.Logging.Common;
using CRUD.WebAPI;
using FluentNHibernate.Cfg.Db;
using Swashbuckle.AspNetCore.SwaggerUI;
using Templates.Blazor.NH.API;
using Templates.Blazor.NH.Shared;

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

builder.Services.AddNHibernateDAL(PostgreSQLConfiguration.Standard.ConnectionString(defaultConnectionString),
                                  fluentMappingsAssemblies: new[]
                                                            {
                                                                    typeof(Program).Assembly,
                                                                    typeof(CRUD.Logging.NHibernate.LogMapping).Assembly
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

app.UseMiddleware<ValidationErrorsHandlerMiddleware>();
app.UseMiddleware<ExceptionsHandlerMiddleware>();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

#endregion

app.Run();