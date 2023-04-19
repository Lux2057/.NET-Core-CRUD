namespace EfTests.Core;

#region << Using >>

using CRUD.Core;
using CRUD.CQRS;
using Microsoft.EntityFrameworkCore;

#endregion

public class DeleteEntitiesCommandTests : ReadWriteDispatcherTest
{
    #region Constructors

    public DeleteEntitiesCommandTests(TestDbContext context, IReadWriteDispatcher dispatcher)
            : base(context, dispatcher) { }

    #endregion

    [Fact]
    public async Task Should_delete_entities()
    {
        await this.context.Set<TestEntity>().AddRangeAsync(new[]
                                                           {
                                                                   new TestEntity { Text = Guid.NewGuid().ToString() },
                                                                   new TestEntity { Text = Guid.NewGuid().ToString() },
                                                                   new TestEntity { Text = Guid.NewGuid().ToString() }
                                                           });

        await this.context.SaveChangesAsync();

        var command = new DeleteEntitiesCommand<TestEntity, int>(new[] { 1, 2, 3 });
        await this.dispatcher.PushAsync(command);

        Assert.True(command.Result);

        var entitiesInDb = await this.context.Set<TestEntity>().ToArrayAsync();

        Assert.Empty(entitiesInDb);
    }
}