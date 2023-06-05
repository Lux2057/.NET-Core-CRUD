namespace EfTests.DAL;

#region << Using >>

using CRUD.DAL.EntityFramework;
using EfTests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

#endregion

public class Startup
{
    public virtual void ConfigureServices(IServiceCollection services)
    {
        var connectionString = new ConfigurationBuilder()
                               .SetBasePath(CRUD.Extensions.PathHelper.GetApplicationRootOrDefault())
                               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                               .Build().GetConnectionString("DefaultConnection");

        services.AddEntityFrameworkDAL<TestDbContext>(options => options.UseNpgsql(connectionString));
    }
}