namespace CRUD.CQRS
{
    #region << Using >>

    #region << Using >>

    using System;
    using System.Reflection;
    using CRUD.DAL;
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    #endregion

    #endregion

    public static class ServicesExt
    {
        /// <summary>
        ///     Add all dependencies for EntityFrameworkCore based implementations
        /// </summary>
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
            services.AddScoped(typeof(IReadRepository<>), typeof(EfReadRepository<>));
            services.AddScoped(typeof(IReadWriteRepository<>), typeof(EfReadWriteRepository<>));
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            services.AddScoped<IReadDispatcher, DefaultReadDispatcher>();
            services.AddScoped<IReadWriteDispatcher, DefaultReadWriteDispatcher>();
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