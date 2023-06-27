#region << Using >>

using CRUD.Core;
using CRUD.CQRS;
using CRUD.DAL.EntityFramework;
using CRUD.Logging.Common;
using CRUD.WebAPI;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;
using WebAPI;
using WebAPI_Template_EF;

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

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    using (var context = serviceScope.ServiceProvider.GetService<ApiDbContext>())
    {
        context.Database.EnsureCreated();
        context.Database.Migrate();
    }
}

app.Run();