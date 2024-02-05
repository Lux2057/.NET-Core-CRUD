#region << Using >>

using System.Text;
using CRUD.Core;
using CRUD.CQRS;
using CRUD.DAL.EntityFramework;
using CRUD.Logging.Common;
using CRUD.Logging.EntityFramework;
using CRUD.WebAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Samples.ToDo.API;
using Samples.ToDo.Shared;
using Swashbuckle.AspNetCore.SwaggerUI;

#endregion

var builder = WebApplication.CreateBuilder(args);

#region Services config

/*var jwtSettings = builder.Configuration.GetSection("JWTSettings");
builder.Services
       .AddAuthentication(opt =>
                          {
                              opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                              opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                          })
       .AddJwtBearer(options =>
                     {
                         options.TokenValidationParameters = new TokenValidationParameters
                                                             {
                                                                     ValidateIssuer = true,
                                                                     ValidateAudience = true,
                                                                     ValidateLifetime = true,
                                                                     ValidateIssuerSigningKey = true,

                                                                     ValidIssuer = jwtSettings["validIssuer"],
                                                                     ValidAudience = jwtSettings["validAudience"],
                                                                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["securityKey"]!))
                                                             };
                     });*/


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

MarkEntitiesAsDeletedCommand<UserEntity>.Register(builder.Services);

IsNameUniqueQuery<ProjectEntity>.Register(builder.Services);
IsNameUniqueQuery<TaskEntity>.Register(builder.Services);

DoesEntityExistQuery<UserEntity>.Register(builder.Services);
DoesEntityExistQuery<ProjectEntity>.Register(builder.Services);
DoesEntityExistQuery<StatusEntity>.Register(builder.Services);
DoesEntityExistQuery<TaskEntity>.Register(builder.Services);
DoesEntityExistQuery<TagEntity>.Register(builder.Services);

#endregion

var app = builder.Build();

#region App config

app.UseAuthentication();
app.UseAuthorization();

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
app.UseMiddleware<ExceptionsHandlerMiddleware<AddLogCommand>>();

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