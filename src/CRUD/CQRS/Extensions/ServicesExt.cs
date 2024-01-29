namespace CRUD.CQRS;

#region << Using >>

#region << Using >>

using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

#endregion

#endregion

public static class ServicesExt
{
    /// <summary>
    ///     Adds all CQRS dependencies.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="mediatorAssemblies">Assemblies that contain Queries/Commands and their handlers.</param>
    /// <param name="validatorAssemblies">Assemblies that contain Queries/Commands FluentValidators.</param>
    /// <param name="automapperAssemblies">Assemblies that contains Queries/Commands Automapper.Profiles.</param>
    public static void AddCQRS(this IServiceCollection services,
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