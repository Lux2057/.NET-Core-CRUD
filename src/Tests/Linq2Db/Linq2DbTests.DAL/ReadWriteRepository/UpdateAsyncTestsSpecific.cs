namespace Linq2DbTests.DAL;

#region << Using >>

using CRUD.DAL.Linq2Db;
using Linq2DbTests.Shared;
using LinqToDB;
using LinqToDB.Data;

#endregion

public class UpdateAsyncTestsSpecific : Linq2DbRepositoryTest
{
    #region Constructors

    public UpdateAsyncTestsSpecific(TestDataConnection connection,
                                    ILinq2DbRepository repository)
            : base(connection, repository) { }

    #endregion

    public static IEnumerable<object[]> AssertionData()
    {
        yield return new object[] { new[] { nameof(TestEntity) } };
        yield return new object[] { new[] { $"{nameof(TestEntity)}1", $"{nameof(TestEntity)}2" } };
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public async Task Should_update_an_entity(string[] tableNames)
    {
        foreach (var tableName in tableNames)
        {
            Connection.TryCreateTable<TestEntity>(tableName, true);

            var oldText = Guid.NewGuid().ToString();
            var newText = Guid.NewGuid().ToString();

            var entity = new TestEntity { Text = oldText };

            await Connection.InsertAsync(entity, tableName: tableName);

            entity.Text = newText;
            await Linq2DbRepository.UpdateAsync(entity, tableName: tableName);

            var entitiesInDb = await Connection.GetTable<TestEntity>().TableName(tableName).ToArrayAsync();

            Assert.Single(entitiesInDb);
            Assert.Equal(newText, entitiesInDb[0].Text);
        }
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public async Task Should_ignore_null(string[] tableNames)
    {
        foreach (var tableName in tableNames)
        {
            Connection.TryCreateTable<TestEntity>(tableName, true);

            await Linq2DbRepository.UpdateAsync((TestEntity)null, tableName: tableName);

            var entitiesInDb = await Connection.GetTable<TestEntity>().TableName(tableName).ToArrayAsync();

            Assert.Empty(entitiesInDb);
        }
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public async Task Should_ignore_nonexistent_entity(string[] tableNames)
    {
        foreach (var tableName in tableNames)
        {
            Connection.TryCreateTable<TestEntity>(tableName, true);

            await Linq2DbRepository.UpdateAsync(new TestEntity
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
    public async Task Should_update_several_non_null_entities(string[] tableNames)
    {
        foreach (var tableName in tableNames)
        {
            Connection.TryCreateTable<TestEntity>(tableName, true);

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
                                                   BulkCopyType = BulkCopyType.MultipleRows,
                                                   TableName = tableName
                                           }, entities.Where(r => r != null));

            firstEntity.Text = newText;
            await Linq2DbRepository.UpdateAsync(entities, tableName: tableName);

            var entitiesInDb = await Connection.GetTable<TestEntity>().TableName(tableName).ToArrayAsync();

            Assert.Equal(2, entitiesInDb.Length);
            Assert.True(entitiesInDb.Count(r => r.Text == oldText) == 1);
            Assert.True(entitiesInDb.Count(r => r.Text == newText) == 1);
        }
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public async Task Should_ignore_empty_collection(string[] tableNames)
    {
        foreach (var tableName in tableNames)
        {
            Connection.TryCreateTable<TestEntity>(tableName, true);

            await Linq2DbRepository.UpdateAsync(new TestEntity[] { null }, tableName: tableName);

            var entitiesInDb = await Connection.GetTable<TestEntity>().TableName(tableName).ToArrayAsync();

            Assert.Empty(entitiesInDb);
        }
    }

    [Theory]
    [MemberData(nameof(AssertionData))]
    public async Task Should_be_updated_explicitly_only(string[] tableNames)
    {
        foreach (var tableName in tableNames)
        {
            Connection.TryCreateTable<TestEntity>(tableName, true);

            const string oldText = "old text";
            const string newText = "new text";

            var entities = new[]
                           {
                                   new TestEntity { Text = oldText },
                                   new TestEntity { Text = oldText }
                           };

            await Linq2DbRepository.CreateAsync(entities,
                                                new BulkCopyOptions
                                                {
                                                        TableName = tableName,
                                                        BulkCopyType = BulkCopyType.MultipleRows
                                                });

            foreach (var entity in entities)
                entity.Text = newText;

            entities = Linq2DbRepository.Read<TestEntity>(tableName: tableName).ToArray();

            Assert.True(entities.All(x => x.Text == oldText));
        }
    }
}