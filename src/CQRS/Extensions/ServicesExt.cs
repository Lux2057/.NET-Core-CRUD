namespace CRUD.CQRS
{
    #region << Using >>

    #region << Using >>

    using System;
    using System.Reflection;
    using CRUD.DAL;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Tool.hbm2ddl;

    #endregion

    #endregion

    public static class ServicesExt
    {
        /// <summary>
        ///     Add all dependencies for EntityFrameworkCore based implementations
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dbContextOptions">EntityFrameworkCore DB configuration</param>
        /// <param name="mediatorAssemblies">Assemblies that contain Queries/Commands and their handlers</param>
        /// <param name="validatorAssemblies">Assemblies that contain Queries/Commands FluentValidators</param>
        /// <param name="automapperAssemblies">Assemblies that contains Queries/Commands Automapper.Profiles</param>
        public static void AddEfInfrastructure<TDbContext>(this IServiceCollection services,
                                                           Action<DbContextOptionsBuilder> dbContextOptions,
                                                           Assembly[] mediatorAssemblies,
                                                           Assembly[] validatorAssemblies,
                                                           Assembly[] automapperAssemblies)
                where TDbContext : DbContext, IEfDbContext
        {
            services.AddDbContext<TDbContext>(dbContextOptions);
            services.AddScoped(provider => provider.GetService(typeof(TDbContext)) as IEfDbContext);
            services.AddScoped(typeof(IReadRepository), typeof(EfRepository));
            services.AddScoped(typeof(IRepository), typeof(EfRepository));
            services.AddScoped<IScopedUnitOfWork, EfScopedUnitOfWork>();

            services.addCommonServices(mediatorAssemblies: mediatorAssemblies,
                                       validatorAssemblies: validatorAssemblies,
                                       automapperAssemblies: automapperAssemblies);
        }

        /// <summary>
        ///     Add all dependencies for Fluent NHibernate based implementations
        /// </summary>
        /// <param name="services"></param>
        /// <param name="fluentMappingsAssemblies"></param>
        /// <param name="mediatorAssemblies">Assemblies that contain Queries/Commands and their handlers</param>
        /// <param name="validatorAssemblies">Assemblies that contain Queries/Commands FluentValidators</param>
        /// <param name="automapperAssemblies">Assemblies that contains Queries/Commands Automapper.Profiles</param>
        /// <param name="dbConfig">NHibernate DB configuration</param>
        public static void AddNhInfrastructure(this IServiceCollection services,
                                               IPersistenceConfigurer dbConfig,
                                               Assembly[] fluentMappingsAssemblies,
                                               Assembly[] mediatorAssemblies,
                                               Assembly[] validatorAssemblies,
                                               Assembly[] automapperAssemblies)
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
                                                     .ExposeConfiguration(cfg =>
                                                                          {
                                                                              new SchemaUpdate(cfg).Execute(false, true);
                                                                          })
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

            services.addCommonServices(mediatorAssemblies: mediatorAssemblies,
                                       validatorAssemblies: validatorAssemblies,
                                       automapperAssemblies: automapperAssemblies);
        }

        static void addCommonServices(this IServiceCollection services,
                                      Assembly[] mediatorAssemblies,
                                      Assembly[] validatorAssemblies,
                                      Assembly[] automapperAssemblies)
        {
            services.AddScoped<IReadDispatcher, DefaultDispatcher>();
            services.AddScoped<IDispatcher, DefaultDispatcher>();
            services.AddValidatorsFromAssemblies(assemblies: validatorAssemblies, includeInternalTypes: true);
            services.AddAutoMapper(automapperAssemblies);
            services.AddMediatR(config =>
                                {
                                    config.MediatorImplementationType = typeof(ReMediator);
                                    config.RegisterServicesFromAssemblies(mediatorAssemblies);
                                });
        }
    }
}