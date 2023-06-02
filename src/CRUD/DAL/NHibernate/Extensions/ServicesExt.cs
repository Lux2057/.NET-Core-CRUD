namespace CRUD.DAL.NHibernate;

#region << Using >>

using System.Reflection;
using CRUD.DAL.Abstractions;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using global::NHibernate;
using global::NHibernate.Cfg;
using global::NHibernate.Tool.hbm2ddl;
using Microsoft.Extensions.DependencyInjection;

#endregion

public static class ServicesExt
{
    /// <summary>
    ///     Add all dependencies for Fluent NHibernate based DAL implementations
    /// </summary>
    /// <param name="services"></param>
    /// <param name="fluentMappingsAssemblies"></param>
    /// <param name="dbConfig">NHibernate DB configuration</param>
    public static void AddNHibernateDAL(this IServiceCollection services,
                                        IPersistenceConfigurer dbConfig,
                                        Assembly[] fluentMappingsAssemblies)
    {
        services.AddSingleton(_ =>
                              {
                                  var config = new Configuration();

                                  if (dbConfig.GetType() == typeof(PostgreSQLConfiguration))
                                      config.SetNamingStrategy(new DefaultPostgreSqlNamingStrategy());

                                  return Fluently.Configure(config)
                                                 .Database(dbConfig)
                                                 .Mappings(mappings =>
                                                           {
                                                               foreach (var fluentMappingsAssembly in fluentMappingsAssemblies)
                                                                   mappings.FluentMappings.AddFromAssembly(fluentMappingsAssembly);
                                                           })
                                                 .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                                                 .BuildSessionFactory();
                              });

        services.AddScoped(provider =>
                           {
                               var session = provider.GetService<ISessionFactory>()!.OpenSession();
                               session.FlushMode = FlushMode.Manual;

                               return session;
                           });

        services.AddScoped(typeof(IReadRepository), typeof(NhRepository));
        services.AddScoped(typeof(IRepository), typeof(NhRepository));
        services.AddScoped<IScopedUnitOfWork, NhScopedUnitOfWork>();
    }
}