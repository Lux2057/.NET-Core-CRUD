namespace CRUD.Example
{
    #region << Using >>

    using CRUD.Core;
    using CRUD.CQRS;
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

            var dbConnectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ExampleDbContext>(options =>
                                                    {
                                                        options.UseNpgsql(dbConnectionString);
                                                        options.UseLazyLoadingProxies();
                                                    });

            services.AddEfInfrastructure<ExampleDbContext>(mediatorAssemblies: new[]
                                                                               {
                                                                                       typeof(CreateOrUpdateEntitiesCommand<,>).Assembly
                                                                               },
                                                           validatorAssemblies: new[]
                                                                                {
                                                                                        typeof(ExampleEntity).Assembly
                                                                                },
                                                           automapperAssemblies: new[]
                                                                                 {
                                                                                         typeof(ExampleEntity).Assembly
                                                                                 });

            services.AddEntityCRUD<ExampleEntity, ExampleDto>();
        }

        public void Configure(IApplicationBuilder app, ExampleDbContext dbContext)
        {
            dbContext.Database.Migrate();

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
                             {
                                 c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Example");
                                 c.RoutePrefix = "swagger";
                                 c.DocExpansion(DocExpansion.None);
                             });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
                             {
                                 endpoints.MapControllers();
                             });
        }
    }
}