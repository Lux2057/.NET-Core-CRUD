namespace CRUD.DAL.EntityFramework;

#region << Using >>

using CRUD.DAL.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

#endregion

public static class ServicesExt
{
    /// <summary>
    ///     Add all dependencies for EntityFrameworkCore based implementations.
    /// </summary>
    public static void AddEntityFrameworkDAL<TDbContext>(this IServiceCollection services,
                                                         Action<DbContextOptionsBuilder> dbContextOptions)
            where TDbContext : DbContext, IEfDbContext
    {
        services.AddDbContext<TDbContext>(dbContextOptions);
        services.AddScoped(provider => provider.GetService(typeof(TDbContext)) as IEfDbContext);
        services.AddScoped(typeof(IReadRepository), typeof(EfRepository));
        services.AddScoped(typeof(IRepository), typeof(EfRepository));
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
    }
}