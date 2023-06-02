﻿namespace NhTests.DAL;

#region << Using >>

using CRUD.DAL.NHibernate;
using FluentNHibernate.Cfg.Db;
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

        services.AddNHibernateDAL(dbConfig: PostgreSQLConfiguration.Standard.ConnectionString(connectionString),
                                  fluentMappingsAssemblies: new[] { typeof(TestEntity.Mapping).Assembly },
                                  dbSchemaMode: NhDbSchemaMode.DropCreate,
                                  useScopedSessionFactory: true);
    }
}