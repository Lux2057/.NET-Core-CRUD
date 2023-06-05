namespace NhTests.Core;

#region << Using >>

using CRUD.Core;
using CRUD.CQRS;
using NHibernate;
using NhTests.Shared;

#endregion

public class DeleteEntitiesCommandTests : DispatcherTest
{
    #region Constructors

    public DeleteEntitiesCommandTests(ISessionFactory sessionFactory, IDispatcher dispatcher)
            : base(sessionFactory, dispatcher) { }

    #endregion

    [Fact]
    public async Task Should_delete_entities()
    {
        await SessionFactory.AddEntitiesAsync(new[]
                                              {
                                                      new TestEntity { Text = Guid.NewGuid().ToString() },
                                                      new TestEntity { Text = Guid.NewGuid().ToString() },
                                                      new TestEntity { Text = Guid.NewGuid().ToString() }
                                              });

        var command = new DeleteEntitiesCommand<TestEntity, int>(new[] { 1, 2, 3 });
        await Dispatcher.PushAsync(command);

        Assert.True(command.Result);

        var entitiesInDb = (await Dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, int, TestEntityDto>())).Items;

        Assert.Empty(entitiesInDb);
    }

    [Fact]
    public async Task Should_throw_exception()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await Dispatcher.PushAsync(new DeleteEntitiesCommand<TestEntity, int>(Array.Empty<int>())));
    }
}