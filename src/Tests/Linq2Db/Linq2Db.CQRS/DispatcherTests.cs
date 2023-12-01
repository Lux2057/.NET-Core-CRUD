namespace Linq2Db.CQRS;

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
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        var text = Guid.NewGuid().ToString();
        var command1 = new AddOrUpdateTestEntityCommand { Text = text };
        var command2 = new AddOrUpdateTestEntityCommand { Text = text };
        var command3 = new AddOrUpdateTestEntityCommand { Text = text };
        await Dispatcher.PushAsync(command1);
        await Dispatcher.PushAsync(command2);
        await Dispatcher.PushAsync(command3);

        var entitiesInDb = await Connection.GetTable<TestEntity>().ToArrayAsync();

        Assert.Equal(3, entitiesInDb.Length);
        Assert.True(entitiesInDb.SingleOrDefault(r => r.Id == command1.Result) != null);
        Assert.True(entitiesInDb.SingleOrDefault(r => r.Id == command2.Result) != null);
        Assert.True(entitiesInDb.SingleOrDefault(r => r.Id == command3.Result) != null);

        var dtos = await Dispatcher.QueryAsync(new GetTestEntitiesByIdsQueryBase(Array.Empty<string>()));

        Assert.Equal(3, dtos.Length);
        Assert.True(dtos.All(x => x.Text == text));
    }

    [Fact]
    public async Task Should_throw_validation_exception()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        await Assert.ThrowsAsync<ValidationException>(async () => await Dispatcher.QueryAsync(new GetTestEntitiesByIdsQueryBase(null)));
    }

    [Fact]
    public async Task Should_throw_test_exception()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        await Assert.ThrowsAsync<Exception>(async () => await Dispatcher.QueryAsync(new TestThrowingExceptionQueryBase()));
        await Assert.ThrowsAsync<Exception>(async () => await Dispatcher.PushAsync(new TestThrowingExceptionCommand()));
    }

    [Fact]
    public async Task Should_rollback_changes_in_the_command()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        await Assert.ThrowsAsync<Exception>(async () => await Dispatcher.PushAsync(new TestRollbackChangesCommand()));

        var entitiesInDb = await Connection.GetTable<TestEntity>().ToArrayAsync();

        Assert.Empty(entitiesInDb);
    }

    [Fact]
    public async Task Should_create_an_entity_by_generic_command()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        var text = Guid.NewGuid().ToString();
        await Dispatcher.PushAsync(new TestGenericCommand<TestEntity> { Text = text });

        var entitiesInDb = await Dispatcher.QueryAsync(new GetTestEntitiesTestQuery());

        Assert.Single(entitiesInDb);
        Assert.Equal(text, entitiesInDb.Single().Text);
    }

    [Fact]
    public async Task Should_roll_back_changes_in_the_whole_scope()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        await Dispatcher.PushAsync(new AddOrUpdateTestEntityCommand { Text = "test" });

        await Assert.ThrowsAnyAsync<Exception>(async () => await Dispatcher.PushAsync(new TestEntitiesCreationRollbackCommand()));

        var entitiesInDb = await Connection.GetTable<TestEntity>().ToArrayAsync();

        Assert.Empty(entitiesInDb);
    }
}