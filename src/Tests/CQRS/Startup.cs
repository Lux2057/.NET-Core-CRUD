namespace Tests.CQRS;

#region << Using >>

using CRUD.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tests.Models;

#endregion

public class Startup
{
    public virtual void ConfigureServices(IServiceCollection services)
    {
        var rootPath = PathHelper.GetApplicationRoot();

        var connectionString = new ConfigurationBuilder()
                               .SetBasePath(rootPath)
                               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                               .Build().GetConnectionString("DefaultConnection");

        services.AddDbContext<TestDbContext>(options =>
                                             {
                                                 options.UseNpgsql(connectionString);
                                                 options.EnableSensitiveDataLogging();
                                             });

        var currentAssembly = new[]
                              {
                                      typeof(Startup).Assembly,
                                      typeof(TestEntity).Assembly
                              };
        services.AddEfInfrastructure<TestDbContext>(mediatorAssemblies: currentAssembly, 
                                                    validatorAssemblies: currentAssembly, 
                                                    automapperAssemblies: currentAssembly);
    }
}