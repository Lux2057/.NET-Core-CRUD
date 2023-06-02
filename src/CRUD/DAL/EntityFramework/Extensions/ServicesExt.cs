namespace CRUD.DAL.EntityFramework;

#region << Using >>

using CRUD.DAL.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

#endregion

public static class ServicesExt
{
    /// <summary>
    ///     Add all dependencies for EntityFrameworkCore based implementations
    /// </summary>
    /// <param name="services"></param>
    /// <param name="dbContextOptions">EntityFrameworkCore DB configuration</param>
    public static void AddEntityFrameworkDAL<TDbContext>(this IServiceCollection services,
                                                         Action<DbContextOptionsBuilder> dbContextOptions)
            where TDbContext : DbContext, IEfDbContext
    {
        services.AddDbContext<TDbContext>(dbContextOptions);
        services.AddScoped(provider => provider.GetService(typeof(TDbContext)) as IEfDbContext);
        services.AddScoped(typeof(IReadRepository), typeof(EfRepository));
        services.AddScoped(typeof(IRepository), typeof(EfRepository));
        services.AddScoped<IScopedUnitOfWork, EfScopedUnitOfWork>();
    }
}