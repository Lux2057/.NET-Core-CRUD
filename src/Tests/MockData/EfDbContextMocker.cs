namespace CRUD.Tests;

#region << Using >>

using Microsoft.EntityFrameworkCore;

#endregion

public static class EfDbContextMocker
{
    public static void ExecuteWithDbContext(Action<TestDbContext> action)
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
                      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                      .Options;

        using (var context = new TestDbContext(options))
        {
            action.Invoke(context);
        }
    }
}