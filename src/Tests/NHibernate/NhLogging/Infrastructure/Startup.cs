namespace NhTests.Logging;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.NHibernate;
using CRUD.Logging.Common;
using CRUD.Logging.NHibernate;
using Extensions;
using FluentNHibernate.Cfg.Db;
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
                                      typeof(LogMapping).Assembly,
                                      typeof(AddLogCommand).Assembly,
                                      typeof(GetLogsQuery).Assembly
                              };

        services.AddNHibernateDAL(dbConfig: PostgreSQLConfiguration.Standard.ConnectionString(connectionString),
                                  fluentMappingsAssemblies: new[] { typeof(LogMapping).Assembly },
                                  dbSchemaMode: NhDbSchemaMode.DropCreate,
                                  useScopedSessionFactory: true);

        services.AddCQRS(mediatorAssemblies: currentAssembly,
                         validatorAssemblies: currentAssembly,
                         automapperAssemblies: currentAssembly);
    }
}