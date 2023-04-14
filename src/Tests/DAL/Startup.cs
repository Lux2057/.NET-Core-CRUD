namespace Tests.DAL;

#region << Using >>

using CRUD.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tests.Models;

#endregion

public class Startup
{
    #region Constants

    private const string dbConnectionString = "Host=localhost;Port=5432;Database=CRUD_Test;Username=postgres;Password=1";

    #endregion

    public virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped(provider => provider.GetService(typeof(TestDbContext)) as IEfDbContext);
        services.AddScoped(typeof(IReadRepository<>), typeof(EfReadRepository<>));
        services.AddScoped(typeof(IReadWriteRepository<>), typeof(EfReadWriteRepository<>));
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddDbContext<TestDbContext>(options => options.UseNpgsql(dbConnectionString));
    }
}