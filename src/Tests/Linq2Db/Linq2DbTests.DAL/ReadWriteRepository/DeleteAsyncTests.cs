namespace Linq2DbTests.DAL;

#region << Using >>

using CRUD.DAL.Linq2Db;
using Linq2DbTests.Shared;
using LinqToDB;
using LinqToDB.Data;

#endregion

public class DeleteAsyncTests : Linq2DbRepositoryTest
{
    #region Constructors

    public DeleteAsyncTests(TestDataConnection connection, ILinq2DbRepository repository)
            : base(connection, repository) { }

    #endregion

    [Fact]
    public async Task Should_ignore_null_entity()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        await Connection.InsertAsync(new TestEntity { Text = Guid.NewGuid().ToString() });

        await Repository.DeleteAsync((TestEntity)null);

        var entitiesInDb = await Connection.GetTable<TestEntity>().ToArrayAsync();

        Assert.Single(entitiesInDb);
    }

    [Fact]
    public async Task Should_delete_an_entity()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        var entity = new TestEntity { Text = Guid.NewGuid().ToString() };

        await Connection.InsertAsync(entity);

        await Repository.DeleteAsync(entity);

        var entitiesInDb = await Connection.GetTable<TestEntity>().ToArrayAsync();

        Assert.Empty(entitiesInDb);
    }

    [Fact]
    public async Task Should_ignore_an_not_existing_entity()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        await Repository.DeleteAsync(new TestEntity
                                     {
                                             Id = Guid.NewGuid().ToString(),
                                             Text = Guid.NewGuid().ToString()
                                     });

        var entitiesInDb = await Connection.GetTable<TestEntity>().ToArrayAsync();

        Assert.Empty(entitiesInDb);
    }

    [Fact]
    public async Task Should_delete_several_non_null_entities()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        var entities = new[]
                       {
                               new TestEntity { Text = Guid.NewGuid().ToString() },
                               new TestEntity { Text = Guid.NewGuid().ToString() },
                               null
                       };

        await Repository.CreateAsync(entities);

        await Repository.DeleteAsync(entities);

        var entitiesInDb = await Connection.GetTable<TestEntity>().ToArrayAsync();

        Assert.Empty(entitiesInDb);
    }

    [Fact]
    public async Task Should_ignore_empty_collection()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        await Repository.DeleteAsync(new TestEntity[] { null });

        var entitiesInDb = await Connection.GetTable<TestEntity>().ToArrayAsync();

        Assert.Empty(entitiesInDb);
    }
}