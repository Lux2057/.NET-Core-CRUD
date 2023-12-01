namespace Linq2DbTests.DAL;

#region << Using >>

using CRUD.DAL.Linq2Db;
using Linq2DbTests.Shared;
using LinqToDB;
using LinqToDB.Data;

#endregion

public class UpdateAsyncTests : Linq2DbRepositoryTest
{
    #region Constructors

    public UpdateAsyncTests(TestDataConnection connection,
                            ILinq2DbRepository repository)
            : base(connection, repository) { }

    #endregion

    [Fact]
    public async Task Should_update_an_entity()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        var oldText = Guid.NewGuid().ToString();
        var newText = Guid.NewGuid().ToString();

        var entity = new TestEntity { Text = oldText };

        await Connection.InsertAsync(entity);

        entity.Text = newText;
        await Repository.UpdateAsync(entity);

        var entitiesInDb = await Connection.GetTable<TestEntity>().ToArrayAsync();

        Assert.Single(entitiesInDb);
        Assert.Equal(newText, entitiesInDb[0].Text);
    }

    [Fact]
    public async Task Should_ignore_null()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        await Repository.UpdateAsync((TestEntity)null);

        var entitiesInDb = await Connection.GetTable<TestEntity>().ToArrayAsync();

        Assert.Empty(entitiesInDb);
    }

    [Fact]
    public async Task Should_ignore_nonexistent_entity()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        await Repository.UpdateAsync(new TestEntity
                                     {
                                             Id = Guid.NewGuid().ToString(),
                                             Text = Guid.NewGuid().ToString()
                                     });

        var entitiesInDb = await Connection.GetTable<TestEntity>().ToArrayAsync();

        Assert.Empty(entitiesInDb);
    }

    [Fact]
    public async Task Should_update_several_non_null_entities()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        var oldText = Guid.NewGuid().ToString();
        var newText = Guid.NewGuid().ToString();

        var firstEntity = new TestEntity { Text = oldText };
        var entities = new[]
                       {
                               firstEntity,
                               new TestEntity { Text = oldText },
                               null
                       };

        await Connection.BulkCopyAsync(new BulkCopyOptions
                                       {
                                               BulkCopyType = BulkCopyType.MultipleRows
                                       }, entities.Where(r => r != null));

        firstEntity.Text = newText;
        await Repository.UpdateAsync(entities);

        var entitiesInDb = await Connection.GetTable<TestEntity>().ToArrayAsync();

        Assert.Equal(2, entitiesInDb.Length);
        Assert.True(entitiesInDb.Count(r => r.Text == oldText) == 1);
        Assert.True(entitiesInDb.Count(r => r.Text == newText) == 1);
    }

    [Fact]
    public async Task Should_ignore_empty_collection()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        await Repository.UpdateAsync(new TestEntity[] { null });

        var entitiesInDb = await Connection.GetTable<TestEntity>().ToArrayAsync();

        Assert.Empty(entitiesInDb);
    }

    [Fact]
    public async Task Should_be_updated_explicitly_only()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        const string oldText = "old text";
        const string newText = "new text";

        var entities = new[]
                       {
                               new TestEntity { Text = oldText },
                               new TestEntity { Text = oldText }
                       };

        await Repository.CreateAsync(entities);

        foreach (var entity in entities)
            entity.Text = newText;

        entities = Repository.Read<TestEntity>().ToArray();

        Assert.True(entities.All(x => x.Text == oldText));
    }
}