namespace Linq2DbTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using CRUD.DAL.Linq2Db;
using Linq2DbTests.Shared;
using LinqToDB;
using LinqToDB.Data;

#endregion

public class ReadTestsSpecific : Linq2DbReadRepositoryTest
{
    #region Constructors

    public ReadTestsSpecific(ILinq2DbRepository repository, TestDataConnection connection)
            : base(repository, connection) { }

    #endregion

    public static IEnumerable<object[]> AssertionData()
    {
        yield return new object[] { new[] { nameof(TestEntity) } };
        yield return new object[] { new[] { $"{nameof(TestEntity)}1", $"{nameof(TestEntity)}2" } };
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public async Task Should_return_entity(string[] tableNames)
    {
        foreach (var tableName in tableNames)
        {
            Connection.TryCreateTable<TestEntity>(tableName, true);

            var text = Guid.NewGuid().ToString();

            var testEntity = new TestEntity { Text = text };

            await Connection.InsertAsync(testEntity, tableName: tableName);

            var entities = await Linq2DbRepository.Read<TestEntity>(tableName: tableName).ToArrayAsync();

            Assert.Single(entities);
            Assert.Equal(testEntity.Id, entities[0].Id);
            Assert.Equal(text, entities[0].Text);
        }
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public async Task Should_return_entity_by_id(string[] tableNames)
    {
        foreach (var tableName in tableNames)
        {
            Connection.TryCreateTable<TestEntity>(tableName, true);

            var text = Guid.NewGuid().ToString();

            var testEntity = new TestEntity { Text = text };

            await Connection.InsertAsync(testEntity, tableName: tableName);

            var testEntities = Linq2DbRepository.Read<TestEntity>(tableName: tableName).ToArray();
            Assert.Single(testEntities);

            var id = testEntities[0].Id;

            var entityById = Linq2DbRepository.Read(new FindEntityByStringId<TestEntity>(id), tableName: tableName).Single();

            Assert.Equal(id, entityById.Id);
            Assert.Equal(text, entityById.Text);

            entityById = Linq2DbRepository.Read(new FindEntitiesByIds<TestEntity, string>(new[] { id }), tableName: tableName).Single();

            Assert.Equal(id, entityById.Id);
            Assert.Equal(text, entityById.Text);
        }
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public async Task Should_return_entities_by_text_spec(string[] tableNames)
    {
        foreach (var tableName in tableNames)
        {
            Connection.TryCreateTable<TestEntity>(tableName, true);

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
                                                   TableName = tableName
                                           }, testEntities);

            Assert.Equal(3, Linq2DbRepository.Read<TestEntity>(tableName: tableName).Count());
            Assert.Equal(2, Linq2DbRepository.Read(new TestByTextSpecification(text1), tableName: tableName).Count());
            Assert.Equal(1, Linq2DbRepository.Read(new TestByTextSpecification(text2), tableName: tableName).Count());
        }
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public async Task Should_return_entities_by_ids_spec(string[] tableNames)
    {
        foreach (var tableName in tableNames)
        {
            Connection.TryCreateTable<TestEntity>(tableName, true);

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
                                                   TableName = tableName
                                           }, testEntities);

            Assert.Equal(3, Linq2DbRepository.Read(new FindEntitiesByIds<TestEntity, string>(testEntities.Select(r => r.Id)), tableName: tableName).Count());
        }
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public async Task Should_return_entities_ordered_by_ids(string[] tableNames)
    {
        foreach (var tableName in tableNames)
        {
            Connection.TryCreateTable<TestEntity>(tableName, true);

            var text = Guid.NewGuid().ToString();

            var testEntities = new[]
                               {
                                       new TestEntity { Text = text },
                                       new TestEntity { Text = text },
                                       new TestEntity { Text = text }
                               };

            await Connection.BulkCopyAsync(new BulkCopyOptions
                                           {
                                                   BulkCopyType = BulkCopyType.MultipleRows,
                                                   TableName = tableName
                                           }, testEntities);

            var orderedTestEntities = testEntities.OrderBy(r => r.Id).ToArray();

            var entitiesInDb = Linq2DbRepository.Read(orderSpecifications: new[] { new OrderById<TestEntity, string>(false) }, tableName: tableName).ToArray();

            Assert.Equal(3, entitiesInDb.Length);
            Assert.Equal(orderedTestEntities[0].Id, entitiesInDb[0].Id);
            Assert.Equal(orderedTestEntities[1].Id, entitiesInDb[1].Id);
            Assert.Equal(orderedTestEntities[2].Id, entitiesInDb[2].Id);

            entitiesInDb = Linq2DbRepository.Read(orderSpecifications: new[] { new OrderById<TestEntity, string>(true) }, tableName: tableName).ToArray();

            orderedTestEntities = testEntities.OrderByDescending(r => r.Id).ToArray();

            Assert.Equal(3, entitiesInDb.Length);
            Assert.Equal(orderedTestEntities[0].Id, entitiesInDb[0].Id);
            Assert.Equal(orderedTestEntities[1].Id, entitiesInDb[1].Id);
            Assert.Equal(orderedTestEntities[2].Id, entitiesInDb[2].Id);
        }
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public async Task Should_return_entities_ordered_by_text_then_by_ids(string[] tableNames)
    {
        foreach (var tableName in tableNames)
        {
            Connection.TryCreateTable<TestEntity>(tableName, true);

            var testEntities = new[]
                               {
                                       new TestEntity { Text = 1.ToString() },
                                       new TestEntity { Text = 2.ToString() },
                                       new TestEntity { Text = 2.ToString() }
                               };

            await Connection.BulkCopyAsync(new BulkCopyOptions
                                           {
                                                   BulkCopyType = BulkCopyType.MultipleRows,
                                                   TableName = tableName
                                           }, testEntities);

            var orderedTestEntities = testEntities.OrderByDescending(r => r.Text).ThenBy(r => r.Id).ToArray();

            var entitiesInDb = Linq2DbRepository.Read(orderSpecifications: new OrderSpecification<TestEntity>[]
                                                                           {
                                                                                   new OrderByTextTestSpecification(true),
                                                                                   new OrderById<TestEntity, string>(false)
                                                                           }, tableName: tableName).ToArray();

            Assert.Equal(3, entitiesInDb.Length);
            Assert.Equal(orderedTestEntities[0].Id, entitiesInDb[0].Id);
            Assert.Equal(orderedTestEntities[1].Id, entitiesInDb[1].Id);
            Assert.Equal(orderedTestEntities[2].Id, entitiesInDb[2].Id);

            entitiesInDb = Linq2DbRepository.Read(orderSpecifications: new OrderSpecification<TestEntity>[]
                                                                       {
                                                                               new OrderByTextTestSpecification(true),
                                                                               new OrderById<TestEntity, string>(true)
                                                                       }, tableName: tableName).ToArray();

            orderedTestEntities = testEntities.OrderByDescending(r => r.Text).ThenByDescending(r => r.Id).ToArray();

            Assert.Equal(3, entitiesInDb.Length);
            Assert.Equal(orderedTestEntities[0].Id, entitiesInDb[0].Id);
            Assert.Equal(orderedTestEntities[1].Id, entitiesInDb[1].Id);
            Assert.Equal(orderedTestEntities[2].Id, entitiesInDb[2].Id);
        }
    }
}