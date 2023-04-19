﻿namespace CRUD.CQRS
{
    #region << Using >>

    using System.Reflection;
    using CRUD.DAL;
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

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
                                                           Assembly[] mediatorAssemblies,
                                                           Assembly[] validatorAssemblies,
                                                           Assembly[] automapperAssemblies)
                where TDbContext : DbContext, IEfDbContext
        {
            services.AddScoped(provider => provider.GetService(typeof(TDbContext)) as IEfDbContext);
            services.AddScoped(typeof(IReadRepository<>), typeof(EfReadRepository<>));
            services.AddScoped(typeof(IReadWriteRepository<>), typeof(EfReadWriteRepository<>));
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            services.AddScoped<IReadDispatcher, DefaultReadDispatcher>();
            services.AddScoped<IReadWriteDispatcher, DefaultReadWriteDispatcher>();
            services.AddValidatorsFromAssemblies(assemblies: validatorAssemblies, includeInternalTypes: true);
            services.AddAutoMapper(automapperAssemblies);
            services.AddMediatR(config => config.RegisterServicesFromAssemblies(mediatorAssemblies));
        }
    }
}