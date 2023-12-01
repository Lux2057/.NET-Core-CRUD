namespace Linq2DbTests.DAL;

#region << Using >>

using CRUD.DAL.Linq2Db;
using LinqToDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

#endregion

public class Startup
{
    public virtual void ConfigureServices(IServiceCollection services)
    {
        var connectionString = new ConfigurationBuilder()
                               .SetBasePath(Extensions.PathHelper.GetApplicationRootOrDefault())
                               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                               .Build().GetConnectionString("DefaultConnection");

        services.AddLinq2DbDAL<TestDataConnection>((_, options) => options.UsePostgreSQL(connectionString), true);
    }
}