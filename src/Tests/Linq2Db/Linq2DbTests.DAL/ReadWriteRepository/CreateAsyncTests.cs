namespace Linq2DbTests.DAL;

#region << Using >>

using CRUD.DAL.Linq2Db;
using Linq2DbTests.Shared;
using LinqToDB;

#endregion

public class CreateAsyncTests : Linq2DbRepositoryTest
{
    #region Constructors

    public CreateAsyncTests(TestDataConnection connection, ILinq2DbRepository repository)
            : base(connection, repository) { }

    #endregion

    [Fact]
    public async Task Should_create_single_entity()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        var text = Guid.NewGuid().ToString();

        await Repository.CreateAsync(new TestEntity { Text = text });

        var entitiesInDb = await Connection.GetTable<TestEntity>().ToArrayAsync();

        Assert.Single(entitiesInDb);
        Assert.Equal(text, entitiesInDb[0].Text);
    }

    [Fact]
    public async Task Should_ignore_null_entity()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        await Repository.CreateAsync((TestEntity)null);

        var entitiesInDb = await Connection.GetTable<TestEntity>().ToArrayAsync();

        Assert.Empty(entitiesInDb);
    }

    [Fact]
    public async Task Should_create_several_non_null_entities()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        var text = Guid.NewGuid().ToString();

        await Repository.CreateAsync(new TestEntity[]
                                     {
                                             new TestEntity { Text = text },
                                             new TestEntity { Text = text },
                                             null
                                     });

        var entitiesInDb = await Connection.GetTable<TestEntity>().ToArrayAsync();

        Assert.Equal(2, entitiesInDb.Length);
        Assert.True(entitiesInDb.All(r => r.Text == text));
    }

    [Fact]
    public async Task Should_ignore_empty_collection()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        await Repository.CreateAsync(new[] { (TestEntity)null });

        var entitiesInDb = await Connection.GetTable<TestEntity>().ToArrayAsync();

        Assert.Empty(entitiesInDb);
    }
}