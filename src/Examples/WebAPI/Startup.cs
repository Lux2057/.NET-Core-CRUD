namespace Examples.WebAPI
{
    #region << Using >>

    using System;
    using CRUD.Core;
    using CRUD.CQRS;
    using CRUD.WebAPI;
    using FluentNHibernate.Cfg.Db;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Swashbuckle.AspNetCore.SwaggerUI;

    #endregion

    public class Startup
    {
        #region Properties

        public IConfiguration Configuration { get; }

        #endregion

        #region Constructors

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
                                   {
                                       c.EnableAnnotations();
                                       c.OrderActionsBy(r => r.GroupName);
                                   });

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            /*services.AddNhInfrastructure(PostgreSQLConfiguration.Standard.ConnectionString(connectionString),
                                         fluentMappingsAssemblies: new[]
                                                                   {
                                                                           typeof(ExampleEntity).Assembly
                                                                   },
                                         mediatorAssemblies: new[]
                                                             {
                                                                     typeof(CreateOrUpdateEntitiesCommand<,,>).Assembly,
                                                                     typeof(GetExampleTextsByIdsQueryBase).Assembly
                                                             },
                                         validatorAssemblies: new[]
                                                              {
                                                                      typeof(ExampleEntity).Assembly
                                                              },
                                         automapperAssemblies: new[]
                                                               {
                                                                       typeof(ExampleEntity).Assembly
                                                               });*/

            services.AddEfInfrastructure<ExampleDbContext>
                    (dbContextOptions: options =>
                                       {
                                           options.UseNpgsql(connectionString);
                                           options.UseLazyLoadingProxies();
                                           options.EnableSensitiveDataLogging();
                                       },
                     mediatorAssemblies: new[]
                                         {
                                                 typeof(CreateOrUpdateEntitiesCommand<,,>).Assembly,
                                                 typeof(GetExampleTextsByIdsQueryBase).Assembly
                                         },
                     validatorAssemblies: new[]
                                          {
                                                  typeof(ExampleEntity).Assembly
                                          },
                     automapperAssemblies: new[]
                                           {
                                                   typeof(ExampleEntity).Assembly
                                           });

            services.AddEntityCRUD<ExampleEntity, int, ExampleDto>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
                             {
                                 c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Example");
                                 c.RoutePrefix = "swagger";
                                 c.DocExpansion(DocExpansion.None);
                             });

            app.UseRouting();

            app.UseMiddleware<ValidationErrorsHandlerMiddleware>();
            app.UseMiddleware<ExceptionsHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
                             {
                                 endpoints.MapControllers();
                             });
        }
    }
}