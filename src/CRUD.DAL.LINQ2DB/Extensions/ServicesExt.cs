namespace CRUD.DAL.LINQ2DB;

#region << Using >>

using CRUD.DAL.Abstractions;
using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.Data;
using Microsoft.Extensions.DependencyInjection;

#endregion

public static class ServicesExt
{
    /// <summary>
    ///     Add all dependencies for LINQ2DB based implementations.
    /// </summary>
    public static void AddLinq2DbDAL<TDataConnection>(this IServiceCollection services,
                                                      Func<IServiceProvider, DataOptions, DataOptions> options,
                                                      bool useLinq2DbSpecific = false) where TDataConnection : DataConnection

    {
        services.AddLinqToDBContext<TDataConnection>(options);

        if (useLinq2DbSpecific)
        {
            services.AddScoped(typeof(ILinq2DbReadRepository), typeof(Linq2DbRepository));
            services.AddScoped(typeof(ILinq2DbRepository), typeof(Linq2DbRepository));
        }
        else
        {
            services.AddScoped(typeof(IReadRepository), typeof(Linq2DbRepository));
            services.AddScoped(typeof(IRepository), typeof(Linq2DbRepository));
        }

        services.AddScoped<IUnitOfWork, Linq2DbUnitOfWork>();
    }
}