namespace NhTests.CQRS;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.NHibernate;
using Extensions;
using FluentNHibernate.Cfg.Db;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NhTests.Shared;

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

        services.AddNHibernateDAL(dbConfig: PostgreSQLConfiguration.Standard.ConnectionString(connectionString),
                                  fluentMappingsAssemblies: new[] { typeof(TestEntity.Mapping).Assembly },
                                  dbSchemaMode: NhDbSchemaMode.DropCreate,
                                  useScopedSessionFactory: true);

        services.AddCQRS(mediatorAssemblies: currentAssembly,
                         validatorAssemblies: currentAssembly,
                         automapperAssemblies: currentAssembly);

        services.AddTransient(typeof(INotificationHandler<TestGenericCommand<TestEntity>>), typeof(TestGenericCommand<TestEntity>.Handler));
    }
}