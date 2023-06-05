namespace NhTests.CQRS;

#region << Using >>

using CRUD.CQRS;
using FluentValidation;
using NHibernate;
using NhTests.Shared;

#endregion

public class DispatcherTests : DispatcherTest
{
    #region Constructors

    public DispatcherTests(ISessionFactory sessionFactory, IDispatcher dispatcher)
            : base(sessionFactory, dispatcher) { }

    #endregion

    [Fact]
    public async Task Should_return_entities_from_db()
    {
        var text = Guid.NewGuid().ToString();
        var command1 = new AddOrUpdateTestEntityCommand { Text = text };
        var command2 = new AddOrUpdateTestEntityCommand { Text = text };
        var command3 = new AddOrUpdateTestEntityCommand { Text = text };
        await Dispatcher.PushAsync(command1);
        await Dispatcher.PushAsync(command2);
        await Dispatcher.PushAsync(command3);

        Assert.Equal(1, command1.Result);
        Assert.Equal(2, command2.Result);
        Assert.Equal(3, command3.Result);

        var dtos = await Dispatcher.QueryAsync(new GetTestEntitiesByIdsQueryBase(Array.Empty<int>()));

        Assert.Equal(3, dtos.Length);
        Assert.True(dtos.All(x => x.Text == text));
    }

    [Fact]
    public async Task Should_throw_validation_exception()
    {
        await Assert.ThrowsAsync<ValidationException>(async () => await Dispatcher.QueryAsync(new GetTestEntitiesByIdsQueryBase(null)));
    }

    [Fact]
    public async Task Should_throw_test_exception()
    {
        await Assert.ThrowsAsync<Exception>(async () => await Dispatcher.QueryAsync(new TestThrowingExceptionQueryBase()));
        await Assert.ThrowsAsync<Exception>(async () => await Dispatcher.PushAsync(new TestThrowingExceptionCommand()));
    }

    [Fact]
    public async Task Should_rollback_changes_in_the_command()
    {
        await Assert.ThrowsAsync<Exception>(async () => await Dispatcher.PushAsync(new TestRollbackChangesCommand()));

        var entitiesInDb = await SessionFactory.GetEntitiesAsync<TestEntity>();

        Assert.Empty(entitiesInDb);
    }

    [Fact]
    public async Task Should_create_an_entity_by_generic_command()
    {
        var text = Guid.NewGuid().ToString();
        await Dispatcher.PushAsync(new TestGenericCommand<TestEntity> { Text = text });

        var entitiesInDb = await Dispatcher.QueryAsync(new GetTestEntitiesTestQuery());

        Assert.Single(entitiesInDb);
        Assert.Equal(text, entitiesInDb.Single().Text);
    }

    [Fact]
    public async Task Should_roll_back_changes_in_the_whole_scope()
    {
        await Dispatcher.PushAsync(new AddOrUpdateTestEntityCommand { Text = "test" });

        await Assert.ThrowsAnyAsync<Exception>(async () => await Dispatcher.PushAsync(new TestEntitiesCreationRollbackCommand()));

        var entitiesInDb = await SessionFactory.GetEntitiesAsync<TestEntity>();

        Assert.Empty(entitiesInDb);
    }
}