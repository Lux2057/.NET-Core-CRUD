namespace EfTests.DAL;

#region << Using >>

using CRUD.DAL;
using CRUD.Extensions;
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

        services.AddDbContext<TestDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped(provider => provider.GetService(typeof(TestDbContext)) as IEfDbContext);
        services.AddScoped(typeof(IReadRepository<>), typeof(EfReadRepository<>));
        services.AddScoped(typeof(IReadWriteRepository<>), typeof(EfReadWriteRepository<>));
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
    }
}