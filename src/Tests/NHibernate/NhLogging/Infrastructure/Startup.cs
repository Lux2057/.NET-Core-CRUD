namespace EfTests.Logging;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.EntityFramework;
using CRUD.Extensions;
using CRUD.Logging.Common;
using CRUD.Logging.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

#endregion

public class Startup
{
    public virtual void ConfigureServices(IServiceCollection services)
    {
        var connectionString = new ConfigurationBuilder()
                               .SetBasePath(PathHelper.GetApplicationRootOrDefault())
                               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                               .Build().GetConnectionString("DefaultConnection");

        var currentAssembly = new[]
                              {
                                      typeof(LogMapping).Assembly,
                                      typeof(AddLogCommand).Assembly
                              };

        services.AddEntityFrameworkDAL<TestDbContext>(dbContextOptions: options =>
                                                                        {
                                                                            options.UseNpgsql(connectionString);
                                                                            options.EnableSensitiveDataLogging();
                                                                        });

        services.AddCQRS(mediatorAssemblies: currentAssembly,
                         validatorAssemblies: currentAssembly,
                         automapperAssemblies: currentAssembly);
    }
}