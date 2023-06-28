#region << Using >>

using CRUD.Core;
using CRUD.CQRS;
using CRUD.DAL.NHibernate;
using CRUD.Logging.Common;
using CRUD.WebAPI;
using FluentNHibernate.Cfg.Db;
using Swashbuckle.AspNetCore.SwaggerUI;
using Templates.WebAPI.NH;

#endregion

var builder = WebApplication.CreateBuilder(args);

#region DI config

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
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
builder.Services.AddEntityCRUD<SampleEntity, int, SampleDto>();

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
                     {
                         c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Example");
                         c.RoutePrefix = "swagger";
                         c.DocExpansion(DocExpansion.None);
                     });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ValidationErrorsHandlerMiddleware>();
app.UseMiddleware<ExceptionsHandlerMiddleware>();

app.MapControllers();

app.Run();