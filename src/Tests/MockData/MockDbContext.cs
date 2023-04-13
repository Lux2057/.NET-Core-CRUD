namespace CRUD.Tests;

#region << Using >>

using System.Linq.Expressions;
using CRUD.DAL;
using Microsoft.EntityFrameworkCore;

#endregion

public static class MockDbHelper
{
    #region Nested Classes

    public sealed class TestDbContext : DbContext, IEfDbContext
    {
        #region Constructors

        public TestDbContext(DbContextOptions<TestDbContext> options)
                : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TestEntity).Assembly);
        }
    }

    #endregion

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