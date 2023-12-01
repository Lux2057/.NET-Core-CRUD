namespace Linq2Db.CQRS.Specific;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Linq2Db;
using Extensions;
using Linq2DbTests.Shared;
using LinqToDB;
using MediatR;
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
                                      typeof(Startup).Assembly,
                                      typeof(TestEntity).Assembly
                              };

        services.AddLinq2DbDAL<TestDataConnection>((_, options) => options.UsePostgreSQL(connectionString), true);

        services.AddCQRS(mediatorAssemblies: currentAssembly,
                         validatorAssemblies: currentAssembly,
                         automapperAssemblies: currentAssembly);

        services.AddTransient(typeof(INotificationHandler<TestGenericCommand<TestEntity>>), typeof(TestGenericCommand<TestEntity>.Handler));
    }
}