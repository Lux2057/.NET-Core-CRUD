namespace EfTests.CQRS;

#region << Using >>

using CRUD.CQRS;
using CRUD.Extensions;
using MediatR;
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

        services.AddDbContext<TestDbContext>(options =>
                                             {
                                                 options.UseNpgsql(connectionString);
                                                 options.EnableSensitiveDataLogging();
                                             });

        var currentAssembly = new[] { typeof(Startup).Assembly };
        services.AddEfInfrastructure<TestDbContext>(mediatorAssemblies: currentAssembly,
                                                    validatorAssemblies: currentAssembly,
                                                    automapperAssemblies: currentAssembly);

        services.AddTransient(typeof(INotificationHandler<TestGenericCommand<TestEntity>>), typeof(TestGenericCommand<TestEntity>.Handler));
    }
}