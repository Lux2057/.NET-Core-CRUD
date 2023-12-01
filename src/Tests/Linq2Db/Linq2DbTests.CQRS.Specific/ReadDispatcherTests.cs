namespace Linq2Db.CQRS.Specific;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Linq2Db;
using FluentValidation;
using Linq2DbTests.Shared;
using LinqToDB.Data;

#endregion

public class ReadDispatcherTests : ReadDispatcherTest
{
    #region Constructors

    public ReadDispatcherTests(TestDataConnection connection, IReadDispatcher dispatcher)
            : base(connection, dispatcher) { }

    #endregion

    [Fact]
    public async Task Should_return_entities_from_db()
    {
        var tableName = $"{nameof(TestEntity)}_test";

        Connection.TryCreateTable<TestEntity>(tableName, true);

        var text = Guid.NewGuid().ToString();
        await Connection.BulkCopyAsync(new BulkCopyOptions
                                       {
                                               BulkCopyType = BulkCopyType.MultipleRows,
                                               TableName = tableName
                                       },
                                       new[]
                                       {
                                               new TestEntity { Text = text },
                                               new TestEntity { Text = text },
                                               new TestEntity { Text = text }
                                       });

        var dtos = await Dispatcher.QueryAsync(new GetTestEntitiesByIdsQueryBase(ids: Array.Empty<string>(),
                                                                                 tableName: tableName));

        Assert.Equal(3, dtos.Length);
        Assert.True(dtos.All(x => x.Text == text));
    }

    [Fact]
    public async Task Should_throw_validation_exception()
    {
        var tableName = $"{nameof(TestEntity)}_test";

        Connection.TryCreateTable<TestEntity>(tableName, true);

        await Assert.ThrowsAsync<ValidationException>(async () => await Dispatcher.QueryAsync(new GetTestEntitiesByIdsQueryBase(ids: null,
                                                                                                                                tableName: tableName)));
    }

    [Fact]
    public async Task Should_throw_test_exception()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        await Assert.ThrowsAsync<Exception>(async () => await Dispatcher.QueryAsync(new TestThrowingExceptionQueryBase()));
    }
}