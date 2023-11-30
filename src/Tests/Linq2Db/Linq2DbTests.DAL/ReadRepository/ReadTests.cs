namespace Linq2DbTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using CRUD.DAL.Linq2Db;
using Linq2DbTests.Shared;
using LinqToDB;
using LinqToDB.Data;

#endregion

public class ReadTests : ReadRepositoryTest
{
    #region Constructors

    public ReadTests(ILinq2DbRepository repository, TestDataConnection connection)
            : base(repository, connection) { }

    #endregion

    [Fact]
    public async Task Should_return_entity()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        var text = Guid.NewGuid().ToString();

        var testEntity = new TestEntity { Text = text };

        await Connection.InsertAsync(testEntity);

        var entities = await Repository.Read<TestEntity>().ToArrayAsync();

        Assert.Single(entities);
        Assert.Equal(testEntity.Id, entities[0].Id);
        Assert.Equal(text, entities[0].Text);
    }

    [Fact]
    public async Task Should_return_entity_by_id()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        var text = Guid.NewGuid().ToString();

        var testEntity = new TestEntity { Text = text };

        await Connection.InsertAsync(testEntity);

        var testEntities = Repository.Read<TestEntity>().ToArray();
        Assert.Single(testEntities);

        var id = testEntities[0].Id;

        var entityById = Repository.Read(new FindEntityByStringId<TestEntity>(id)).Single();

        Assert.Equal(id, entityById.Id);
        Assert.Equal(text, entityById.Text);

        entityById = Repository.Read(new FindEntitiesByIds<TestEntity, string>(new[] { id })).Single();

        Assert.Equal(id, entityById.Id);
        Assert.Equal(text, entityById.Text);
    }

    [Fact]
    public async Task Should_return_entities_by_text_spec()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        var text1 = Guid.NewGuid().ToString();
        var text2 = Guid.NewGuid().ToString();

        var testEntities = new[]
                           {
                                   new TestEntity { Text = text1 },
                                   new TestEntity { Text = text1 },
                                   new TestEntity { Text = text2 }
                           };

        await Connection.BulkCopyAsync(new BulkCopyOptions
                                       {
                                               BulkCopyType = BulkCopyType.MultipleRows
                                       }, testEntities);

        Assert.Equal(3, Repository.Read<TestEntity>().Count());
        Assert.Equal(2, Repository.Read(new TestByTextSpecification(text1)).Count());
        Assert.Equal(1, Repository.Read(new TestByTextSpecification(text2)).Count());
    }

    [Fact]
    public async Task Should_return_entities_by_ids_spec()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        var text1 = Guid.NewGuid().ToString();
        var text2 = Guid.NewGuid().ToString();

        var testEntities = new[]
                           {
                                   new TestEntity { Text = text1 },
                                   new TestEntity { Text = text1 },
                                   new TestEntity { Text = text2 }
                           };

        await Connection.BulkCopyAsync(new BulkCopyOptions
                                       {
                                               BulkCopyType = BulkCopyType.MultipleRows,
                                       }, testEntities);

        Assert.Equal(3, Repository.Read(new FindEntitiesByIds<TestEntity, string>(testEntities.Select(r => r.Id))).Count());
    }

    [Fact]
    public async Task Should_return_entities_ordered_by_ids()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        var text = Guid.NewGuid().ToString();

        var testEntities = new[]
                           {
                                   new TestEntity { Text = text },
                                   new TestEntity { Text = text },
                                   new TestEntity { Text = text }
                           };

        await Connection.BulkCopyAsync(new BulkCopyOptions
                                       {
                                               BulkCopyType = BulkCopyType.MultipleRows
                                       }, testEntities);

        var orderedTestEntities = testEntities.OrderBy(r => r.Id).ToArray();

        var entitiesInDb = Repository.Read(orderSpecifications: new[] { new OrderById<TestEntity, string>(false) }).ToArray();

        Assert.Equal(3, entitiesInDb.Length);
        Assert.Equal(orderedTestEntities[0].Id, entitiesInDb[0].Id);
        Assert.Equal(orderedTestEntities[1].Id, entitiesInDb[1].Id);
        Assert.Equal(orderedTestEntities[2].Id, entitiesInDb[2].Id);

        entitiesInDb = Repository.Read(orderSpecifications: new[] { new OrderById<TestEntity, string>(true) }).ToArray();

        orderedTestEntities = testEntities.OrderByDescending(r => r.Id).ToArray();

        Assert.Equal(3, entitiesInDb.Length);
        Assert.Equal(orderedTestEntities[0].Id, entitiesInDb[0].Id);
        Assert.Equal(orderedTestEntities[1].Id, entitiesInDb[1].Id);
        Assert.Equal(orderedTestEntities[2].Id, entitiesInDb[2].Id);
    }

    [Fact]
    public async Task Should_return_entities_ordered_by_text_then_by_ids()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        var testEntities = new[]
                           {
                                   new TestEntity { Text = 1.ToString() },
                                   new TestEntity { Text = 2.ToString() },
                                   new TestEntity { Text = 2.ToString() }
                           };

        await Connection.BulkCopyAsync(new BulkCopyOptions
                                       {
                                               BulkCopyType = BulkCopyType.MultipleRows
                                       }, testEntities);

        var orderedTestEntities = testEntities.OrderByDescending(r => r.Text).ThenBy(r => r.Id).ToArray();

        var entitiesInDb = Repository.Read(orderSpecifications: new OrderSpecification<TestEntity>[]
                                                                {
                                                                        new OrderByTextTestSpecification(true),
                                                                        new OrderById<TestEntity, string>(false)
                                                                }).ToArray();

        Assert.Equal(3, entitiesInDb.Length);
        Assert.Equal(orderedTestEntities[0].Id, entitiesInDb[0].Id);
        Assert.Equal(orderedTestEntities[1].Id, entitiesInDb[1].Id);
        Assert.Equal(orderedTestEntities[2].Id, entitiesInDb[2].Id);

        entitiesInDb = Repository.Read(orderSpecifications: new OrderSpecification<TestEntity>[]
                                                            {
                                                                    new OrderByTextTestSpecification(true),
                                                                    new OrderById<TestEntity, string>(true)
                                                            }).ToArray();

        orderedTestEntities = testEntities.OrderByDescending(r => r.Text).ThenByDescending(r => r.Id).ToArray();

        Assert.Equal(3, entitiesInDb.Length);
        Assert.Equal(orderedTestEntities[0].Id, entitiesInDb[0].Id);
        Assert.Equal(orderedTestEntities[1].Id, entitiesInDb[1].Id);
        Assert.Equal(orderedTestEntities[2].Id, entitiesInDb[2].Id);
    }
}