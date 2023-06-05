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
    /// <param name="dbSchemaMode">DB schema usage mode</param>
    /// <param name="useScopedSessionFactory">Register ISessionFactory as scoped service</param>
    public static void AddNHibernateDAL(this IServiceCollection services,
                                        IPersistenceConfigurer dbConfig,
                                        Assembly[] fluentMappingsAssemblies,
                                        NhDbSchemaMode dbSchemaMode = NhDbSchemaMode.Update,
                                        bool useScopedSessionFactory = false)
    {
        Func<IServiceProvider, ISessionFactory> sessionFactory =
                _ =>
                {
                    var config = new Configuration();

                    if (dbConfig.GetType() == typeof(PostgreSQLConfiguration))
                        config.SetNamingStrategy(new DefaultPostgreSqlNamingStrategy());

                    var fluentConfiguration = Fluently.Configure(config)
                                                      .Database(dbConfig)
                                                      .Mappings(mappings =>
                                                                {
                                                                    foreach (var fluentMappingsAssembly in fluentMappingsAssemblies)
                                                                        mappings.FluentMappings.AddFromAssembly(fluentMappingsAssembly);
                                                                });

                    switch (dbSchemaMode)
                    {
                        case NhDbSchemaMode.DropCreate:
                            fluentConfiguration.ExposeConfiguration(cfg =>
                                                                    {
                                                                        new SchemaExport(cfg).Drop(false, true);
                                                                        new SchemaExport(cfg).Create(true, true);
                                                                    });

                            break;

                        case NhDbSchemaMode.Update:
                            fluentConfiguration.ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true));
                            break;

                        case NhDbSchemaMode.UseExisting:
                            break;

                        default:
                            throw new NotImplementedException($"Value '{dbSchemaMode}' in '{typeof(NhDbSchemaMode)}' is not handled!");
                    }

                    return fluentConfiguration.BuildSessionFactory();
                };

        if (useScopedSessionFactory)
            services.AddScoped(sessionFactory);
        else
            services.AddSingleton(sessionFactory);

        services.AddScoped(provider =>
                           {
                               var session = provider.GetService<ISessionFactory>()!.OpenSession();
                               session.FlushMode = FlushMode.Manual;

                               return session;
                           });

        services.AddScoped(typeof(IReadRepository), typeof(NhRepository));
        services.AddScoped(typeof(IRepository), typeof(NhRepository));
        services.AddScoped<IUnitOfWork, NhUnitOfWork>();
    }
}