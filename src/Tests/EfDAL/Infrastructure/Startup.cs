﻿namespace EfTests.DAL;

#region << Using >>

using CRUD.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tests.Models;

#endregion

public class Startup
{
    public virtual void ConfigureServices(IServiceCollection services)
    {
        var connectionString = new ConfigurationBuilder()
                               .SetBasePath(PathHelper.GetApplicationRoot())
                               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                               .Build().GetConnectionString("DefaultConnection");

        services.AddDbContext<TestDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped(provider => provider.GetService(typeof(TestDbContext)) as IEfDbContext);
        services.AddScoped(typeof(IReadRepository<>), typeof(EfReadRepository<>));
        services.AddScoped(typeof(IReadWriteRepository<>), typeof(EfReadWriteRepository<>));
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
    }
}