namespace Linq2DbTests.Core;

#region << Using >>

using CRUD.Core;
using CRUD.CQRS;
using CRUD.DAL.Linq2Db;
using Linq2DbTests.Shared;
using LinqToDB.Data;

#endregion

public class DeleteEntitiesCommandTests : DispatcherTest
{
    #region Constructors

    public DeleteEntitiesCommandTests(TestDataConnection connection, IDispatcher dispatcher) : base(connection, dispatcher) { }

    #endregion

    [Fact]
    public async Task Should_delete_entities()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        var entity1 = new TestEntity { Text = Guid.NewGuid().ToString() };
        var entity2 = new TestEntity { Text = Guid.NewGuid().ToString() };
        var entity3 = new TestEntity { Text = Guid.NewGuid().ToString() };
        await Connection.BulkCopyAsync(new BulkCopyOptions
                                       {
                                               BulkCopyType = BulkCopyType.MultipleRows
                                       },
                                       new[]
                                       {
                                               entity1,
                                               entity2,
                                               entity3
                                       });

        var command = new DeleteEntitiesCommand<TestEntity, string>(new[] { entity1.Id, entity2.Id, entity3.Id });
        await Dispatcher.PushAsync(command);

        Assert.True(command.Result);

        var entitiesInDb = (await Dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, string, TestEntityDto>())).Items;

        Assert.Empty(entitiesInDb);
    }

    [Fact]
    public async Task Should_throw_exception()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await Dispatcher.PushAsync(new DeleteEntitiesCommand<TestEntity, string>(Array.Empty<string>())));
    }
}