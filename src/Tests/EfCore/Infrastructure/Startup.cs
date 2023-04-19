﻿namespace EfTests.Core;

#region << Using >>

using CRUD.Core;
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
        var connectionString = new ConfigurationBuilder()
                               .SetBasePath(PathHelper.GetApplicationRoot())
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
                                      typeof(CreateOrUpdateEntitiesCommand<,,>).Assembly
                              };

        services.AddEfInfrastructure<TestDbContext>(mediatorAssemblies: currentAssembly,
                                                    validatorAssemblies: currentAssembly,
                                                    automapperAssemblies: currentAssembly);

        services.AddEntityCRUD<TestEntity, int, TestEntityDto>();
    }
}