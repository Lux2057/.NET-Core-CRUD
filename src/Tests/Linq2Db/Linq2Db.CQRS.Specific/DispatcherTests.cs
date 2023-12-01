namespace Linq2Db.CQRS.Specific;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Linq2Db;
using FluentValidation;
using Linq2DbTests.Shared;
using LinqToDB;

#endregion

public class DispatcherTests : DispatcherTest
{
    #region Constructors

    public DispatcherTests(TestDataConnection connection, IDispatcher dispatcher)
            : base(connection, dispatcher) { }

    #endregion

    [Fact]
    public async Task Should_return_entities_from_db()
    {
        var tableName = $"{nameof(TestEntity)}_test";

        Connection.TryCreateTable<TestEntity>(tableName, true);

        var text = Guid.NewGuid().ToString();
        var command1 = new AddOrUpdateTestEntityCommand(id: null,
                                                        text: text,
                                                        tableName: tableName);

        var command2 = new AddOrUpdateTestEntityCommand(id: null,
                                                        text: text,
                                                        tableName: tableName);

        var command3 = new AddOrUpdateTestEntityCommand(id: null,
                                                        text: text,
                                                        tableName: tableName);

        await Dispatcher.PushAsync(command1);
        await Dispatcher.PushAsync(command2);
        await Dispatcher.PushAsync(command3);

        var entitiesInDb = await Connection.GetTable<TestEntity>().TableName(tableName).ToArrayAsync();

        Assert.Equal(3, entitiesInDb.Length);
        Assert.True(entitiesInDb.SingleOrDefault(r => r.Id == command1.Result) != null);
        Assert.True(entitiesInDb.SingleOrDefault(r => r.Id == command2.Result) != null);
        Assert.True(entitiesInDb.SingleOrDefault(r => r.Id == command3.Result) != null);

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
        var tableName = $"{nameof(TestEntity)}_test";

        Connection.TryCreateTable<TestEntity>(tableName, true);

        await Assert.ThrowsAsync<Exception>(async () => await Dispatcher.QueryAsync(new TestThrowingExceptionQueryBase()));
        await Assert.ThrowsAsync<Exception>(async () => await Dispatcher.PushAsync(new TestThrowingExceptionCommand()));
    }

    [Fact]
    public async Task Should_rollback_changes_in_the_command()
    {
        var tableName = $"{nameof(TestEntity)}_test";

        Connection.TryCreateTable<TestEntity>(tableName, true);

        await Assert.ThrowsAsync<Exception>(async () => await Dispatcher.PushAsync(new TestRollbackChangesCommand(tableName)));

        var entitiesInDb = await Connection.GetTable<TestEntity>().ToArrayAsync();

        Assert.Empty(entitiesInDb);
    }

    [Fact]
    public async Task Should_create_an_entity_by_generic_command()
    {
        var tableName = $"{nameof(TestEntity)}_test";

        Connection.TryCreateTable<TestEntity>(tableName, true);

        var text = Guid.NewGuid().ToString();
        await Dispatcher.PushAsync(new TestGenericCommand<TestEntity>(text: text,
                                                                      tableName: tableName));

        var entitiesInDb = await Dispatcher.QueryAsync(new GetTestEntitiesTestQuery(tableName));

        Assert.Single(entitiesInDb);
        Assert.Equal(text, entitiesInDb.Single().Text);
    }

    [Fact]
    public async Task Should_roll_back_changes_in_the_whole_scope()
    {
        var tableName = $"{nameof(TestEntity)}_test";

        Connection.TryCreateTable<TestEntity>(tableName, true);

        await Dispatcher.PushAsync(new AddOrUpdateTestEntityCommand(id: null,
                                                                    text: "test",
                                                                    tableName: tableName));

        await Assert.ThrowsAnyAsync<Exception>(async () => await Dispatcher.PushAsync(new TestEntitiesCreationRollbackCommand(tableName)));

        var entitiesInDb = await Connection.GetTable<TestEntity>().ToArrayAsync();

        Assert.Empty(entitiesInDb);
    }
}