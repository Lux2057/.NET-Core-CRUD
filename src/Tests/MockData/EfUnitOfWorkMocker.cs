namespace CRUD.Tests;

#region << Using >>

using CRUD.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

#endregion

public static class EfUnitOfWorkMocker
{
    public static void ExecuteWithUnitOfWork(Action<EfUnitOfWork> action)
    {
        var serviceProvider = new ServiceCollection()
                              .AddScoped(provider => provider.GetService(typeof(TestDbContext)) as IEfDbContext)
                              .AddScoped(typeof(IReadRepository<>), typeof(EfReadRepository<>))
                              .AddScoped(typeof(IReadWriteRepository<>), typeof(EfReadWriteRepository<>))
                              .AddDbContext<TestDbContext>(builder => builder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()))
                              .BuildServiceProvider();

        action.Invoke(new DAL.EfUnitOfWork(serviceProvider));
    }
}