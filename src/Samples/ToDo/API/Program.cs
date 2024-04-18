#region << Using >>

using System.Globalization;
using CRUD.Core;
using CRUD.CQRS;
using CRUD.DAL.EntityFramework;
using CRUD.Logging.Common;
using CRUD.Logging.EntityFramework;
using CRUD.WebAPI;
using Hangfire;
using Hangfire.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Samples.ToDo.API;
using Samples.ToDo.Shared;
using Swashbuckle.AspNetCore.SwaggerUI;

#endregion

var builder = WebApplication.CreateBuilder(args);

#region Services config

var jwtSettings = builder.Configuration.GetSection("JWTSettings").Get<JwtAuthSettings>()!;
builder.Services.AddSingleton(jwtSettings);

builder.Services.AddAuthentication(x =>
                                   {
                                       x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                       x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                                   })
       .AddJwtBearer(x =>
                     {
                         x.UseSecurityTokenValidators = true;
                         x.RequireHttpsMetadata = true;
                         x.SaveToken = true;

                         x.TokenValidationParameters =
                                 new TokenValidationParameters
                                 {
                                         ValidateIssuer = false,
                                         ValidIssuer = jwtSettings.Issuer,
                                         ValidateAudience = true,
                                         ValidAudience = jwtSettings.Audience,
                                         RequireExpirationTime = false,
                                         ValidateLifetime = true,
                                         ClockSkew = TimeSpan.Zero,
                                         IssuerSigningKey = jwtSettings.Secret.GetSecurityKey(),
                                         ValidateIssuerSigningKey = true,
                                 };
                     });

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen(c =>
                               {
                                   c.EnableAnnotations();
                                   c.OrderActionsBy(r => r.GroupName);
                                   c.AddSecurityDefinition("Bearer",
                                                           new OpenApiSecurityScheme
                                                           {
                                                                   Name = "Authorization",
                                                                   Type = SecuritySchemeType.Http,
                                                                   Scheme = "Bearer",
                                                                   BearerFormat = "JWT",
                                                                   In = ParameterLocation.Header
                                                           });

                                   c.AddSecurityRequirement(new OpenApiSecurityRequirement
                                                            {
                                                                    {
                                                                            new OpenApiSecurityScheme
                                                                            {
                                                                                    Reference = new OpenApiReference
                                                                                                {
                                                                                                        Type = ReferenceType.SecurityScheme,
                                                                                                        Id = "Bearer"
                                                                                                }
                                                                            },
                                                                            new string[] { }
                                                                    }
                                                            });
                               });

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var hangfireConnectionString = builder.Configuration.GetConnectionString("ToDoHangfire")!;
builder.Services.AddDbContext<HangfireDbContext>(options =>
                                                 {
                                                     options.UseNpgsql(hangfireConnectionString);
                                                     options.UseLazyLoadingProxies();
                                                 });

builder.Services.AddHangfire(config =>
                             {
                                 config.UseEFCoreStorage(cb => cb.UseNpgsql(hangfireConnectionString),
                                                         new EFCoreStorageOptions
                                                         {
                                                                 CountersAggregationInterval = new TimeSpan(0, 5, 0),
                                                                 DistributedLockTimeout = new TimeSpan(0, 10, 0),
                                                                 JobExpirationCheckInterval = new TimeSpan(0, 30, 0),
                                                                 QueuePollInterval = new TimeSpan(0, 0, 0, 1),
                                                                 Schema = string.Empty,
                                                                 SlidingInvisibilityTimeout = new TimeSpan(0, 5, 0),
                                                         }).UseDatabaseCreator();

                                 config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180);
                             });

builder.Services.AddHangfireServer();

var dataConnectionString = builder.Configuration.GetConnectionString("ToDoData")!;
builder.Services.AddEntityFrameworkDAL<ApiDbContext>(dbContextOptions: options =>
                                                                       {
                                                                           options.UseNpgsql(dataConnectionString);
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

MarkEntitiesAsDeletedCommand<ProjectEntity>.Register(builder.Services);
MarkEntitiesAsDeletedCommand<TaskEntity>.Register(builder.Services);

IsNameUniqueQuery<ProjectEntity>.Register(builder.Services);
IsNameUniqueQuery<TaskEntity>.Register(builder.Services);

DoesEntityExistQuery<UserEntity>.Register(builder.Services);

DoesEntityBelongToUserQuery<ProjectEntity>.Register(builder.Services);
DoesEntityBelongToUserQuery<TaskEntity>.Register(builder.Services);

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

app.UseHangfireDashboard();

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

app.UseRequestLocalization(options =>
                           {
                               var supportedCultures = LocalizationConst.SupportedLanguages.Select(r => new CultureInfo(r.Key)).ToArray();
                               options.DefaultRequestCulture = new RequestCulture(LocalizationConst.DefaultLanguage);
                               options.SupportedCultures = supportedCultures;
                               options.SupportedUICultures = supportedCultures;
                           });

app.UseMiddleware<ExceptionsHandlerMiddleware<AddLogCommand>>();
app.UseMiddleware<ValidationErrorsHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

#endregion

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    await using (var context = serviceScope.ServiceProvider.GetService<HangfireDbContext>()!)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }
}

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    await using (var context = serviceScope.ServiceProvider.GetService<ApiDbContext>()!)
    {
        context.Database.EnsureCreated();
        context.Database.Migrate();
    }
}

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    using (var dispatcher = serviceScope.ServiceProvider.GetService<IDispatcher>()!)
    {
        await dispatcher.PushAsync(new CleanDbRecurrentCommand());
    }
}

app.Run();