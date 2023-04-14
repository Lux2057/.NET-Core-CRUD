namespace Tests.DAL;

#region << Using >>

using CRUD.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

#endregion

public class Startup
{
    #region Constants

    private const string dbConnectionString = "Host=localhost;Port=5432;Database=CRUD_Test;Username=postgres;Password=1";

    #endregion

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient(provider => provider.GetService(typeof(TestDbContext)) as IEfDbContext);
        services.AddTransient(typeof(IReadRepository<>), typeof(EfReadRepository<>));
        services.AddTransient(typeof(IReadWriteRepository<>), typeof(EfReadWriteRepository<>));
        services.AddTransient<IUnitOfWork, EfUnitOfWork>();
        services.AddDbContext<TestDbContext>(options => options.UseNpgsql(dbConnectionString), ServiceLifetime.Transient);
    }
}