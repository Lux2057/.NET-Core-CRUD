namespace Linq2DbTests.DAL;

#region << Using >>

using CRUD.DAL.Linq2Db;
using Linq2DbTests.Shared;
using LinqToDB;

#endregion

public class DeleteAsyncTestsSpecific : Linq2DbRepositoryTest
{
    #region Constructors

    public DeleteAsyncTestsSpecific(TestDataConnection connection, ILinq2DbRepository repository)
            : base(connection, repository) { }

    #endregion

    public static IEnumerable<object[]> AssertionData()
    {
        yield return new object[] { new[] { nameof(TestEntity) } };
        yield return new object[] { new[] { $"{nameof(TestEntity)}1", $"{nameof(TestEntity)}2" } };
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public async Task Should_ignore_null_entity(string[] tableNames)
    {
        foreach (var tableName in tableNames)
        {
            Connection.TryCreateTable<TestEntity>(tableName, true);

            await Connection.InsertAsync(new TestEntity { Text = Guid.NewGuid().ToString() }, tableName: tableName);

            await Linq2DbRepository.DeleteAsync((TestEntity)null, tableName: tableName);

            var entitiesInDb = await Connection.GetTable<TestEntity>().TableName(tableName).ToArrayAsync();

            Assert.Single(entitiesInDb);
        }
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public async Task Should_delete_an_entity(string[] tableNames)
    {
        foreach (var tableName in tableNames)
        {
            Connection.TryCreateTable<TestEntity>(tableName, true);

            var entity = new TestEntity { Text = Guid.NewGuid().ToString() };

            await Connection.InsertAsync(entity, tableName: tableName);

            await Linq2DbRepository.DeleteAsync(entity, tableName: tableName);

            var entitiesInDb = await Connection.GetTable<TestEntity>().TableName(tableName).ToArrayAsync();

            Assert.Empty(entitiesInDb);
        }
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public async Task Should_ignore_an_not_existing_entity(string[] tableNames)
    {
        foreach (var tableName in tableNames)
        {
            Connection.TryCreateTable<TestEntity>(tableName, true);

            await Linq2DbRepository.DeleteAsync(new TestEntity
                                                {
                                                        Id = Guid.NewGuid().ToString(),
                                                        Text = Guid.NewGuid().ToString()
                                                }, tableName: tableName);

            var entitiesInDb = await Connection.GetTable<TestEntity>().TableName(tableName).ToArrayAsync();

            Assert.Empty(entitiesInDb);
        }
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public async Task Should_delete_several_non_null_entities(string[] tableNames)
    {
        foreach (var tableName in tableNames)
        {
            Connection.TryCreateTable<TestEntity>(tableName, true);

            var entities = new[]
                           {
                                   new TestEntity { Text = Guid.NewGuid().ToString() },
                                   new TestEntity { Text = Guid.NewGuid().ToString() },
                                   null
                           };

            await Linq2DbRepository.CreateAsync(entities, tableName: tableName);

            await Linq2DbRepository.DeleteAsync(entities, tableName: tableName);

            var entitiesInDb = await Connection.GetTable<TestEntity>().TableName(tableName).ToArrayAsync();

            Assert.Empty(entitiesInDb);
        }
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public async Task Should_ignore_empty_collection(string[] tableNames)
    {
        foreach (var tableName in tableNames)
        {
            Connection.TryCreateTable<TestEntity>(tableName, true);

            await Linq2DbRepository.DeleteAsync(new TestEntity[] { null }, tableName: tableName);

            var entitiesInDb = await Connection.GetTable<TestEntity>().TableName(tableName).ToArrayAsync();

            Assert.Empty(entitiesInDb);
        }
    }
}