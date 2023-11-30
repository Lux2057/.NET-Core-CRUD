namespace Linq2DbTests.DAL;

#region << Using >>

using CRUD.DAL.Linq2Db;
using Linq2DbTests.Shared;
using LinqToDB;

#endregion

public class CreateAsyncTestsSpecific : Linq2DbRepositoryTest
{
    #region Constructors

    public CreateAsyncTestsSpecific(TestDataConnection connection, ILinq2DbRepository repository)
            : base(connection, repository) { }

    #endregion

    public static IEnumerable<object[]> AssertionData()
    {
        yield return new object[] { new[] { nameof(TestEntity) } };
        yield return new object[] { new[] { $"{nameof(TestEntity)}1", $"{nameof(TestEntity)}2" } };
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public async Task Should_create_single_entity(string[] tableNames)
    {
        foreach (var tableName in tableNames)
        {
            Connection.TryCreateTable<TestEntity>(tableName, true);

            var text = Guid.NewGuid().ToString();

            await Linq2DbRepository.CreateAsync(new TestEntity { Text = text }, tableName: tableName);

            var entitiesInDb = await Connection.GetTable<TestEntity>().TableName(tableName).ToArrayAsync();

            Assert.Single(entitiesInDb);
            Assert.Equal(text, entitiesInDb[0].Text);
        }
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public async Task Should_ignore_null_entity(string[] tableNames)
    {
        foreach (var tableName in tableNames)
        {
            Connection.TryCreateTable<TestEntity>(tableName, true);

            await Linq2DbRepository.CreateAsync((TestEntity)null, tableName: tableName);

            var entitiesInDb = await Connection.GetTable<TestEntity>().TableName(tableName).ToArrayAsync();

            Assert.Empty(entitiesInDb);
        }
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public async Task Should_create_several_non_null_entities(string[] tableNames)
    {
        foreach (var tableName in tableNames)
        {
            Connection.TryCreateTable<TestEntity>(tableName, true);

            var text = Guid.NewGuid().ToString();

            await Linq2DbRepository.CreateAsync(new TestEntity[]
                                                {
                                                        new TestEntity { Text = text },
                                                        new TestEntity { Text = text },
                                                        null
                                                }, tableName: tableName);

            var entitiesInDb = await Connection.GetTable<TestEntity>().TableName(tableName).ToArrayAsync();

            Assert.Equal(2, entitiesInDb.Length);
            Assert.True(entitiesInDb.All(r => r.Text == text));
        }
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public async Task Should_ignore_empty_collection(string[] tableNames)
    {
        foreach (var tableName in tableNames)
        {
            Connection.TryCreateTable<TestEntity>(tableName, true);

            await Linq2DbRepository.CreateAsync(new[] { (TestEntity)null }, tableName: tableName);

            var entitiesInDb = await Connection.GetTable<TestEntity>().TableName(tableName).ToArrayAsync();

            Assert.Empty(entitiesInDb);
        }
    }
}