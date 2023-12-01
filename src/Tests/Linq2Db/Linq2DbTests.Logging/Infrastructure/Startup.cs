namespace Linq2DbTests.Logging;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Linq2Db;
using CRUD.Logging.Linq2Db;
using Extensions;
using LinqToDB;
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
                                      typeof(LogEntity).Assembly,
                                      typeof(AddLogCommand).Assembly,
                                      typeof(GetLogsQuery).Assembly
                              };

        services.AddLinq2DbDAL<TestDataConnection>((_, options) => options.UsePostgreSQL(connectionString));

        services.AddCQRS(mediatorAssemblies: currentAssembly,
                         validatorAssemblies: currentAssembly,
                         automapperAssemblies: currentAssembly);
    }
}