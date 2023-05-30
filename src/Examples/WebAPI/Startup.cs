namespace Examples.WebAPI
{
    #region << Using >>

    using CRUD.Core;
    using CRUD.CQRS;
    using CRUD.WebAPI;
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

            services.AddEfInfrastructure<ExampleDbContext>
                    (dbContextOptions: options =>
                                       {
                                           options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
                                           options.UseLazyLoadingProxies();
                                           options.EnableSensitiveDataLogging();
                                       },
                     mediatorAssemblies: new[]
                                         {
                                                 typeof(CreateOrUpdateEntitiesCommand<,,>).Assembly,
                                                 typeof(GetExampleTextsByIdsQuery).Assembly
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

        public void Configure(IApplicationBuilder app, ExampleDbContext dbContext)
        {
            dbContext.Database.Migrate();

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