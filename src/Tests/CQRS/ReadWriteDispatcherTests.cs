namespace EfTests.CQRS;

#region << Using >>

using CRUD.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Tests.Models;

#endregion

public class ReadWriteDispatcherTests : EfReadWriteDispatcherTest
{
    #region Constructors

    public ReadWriteDispatcherTests(TestDbContext context, IReadWriteDispatcher dispatcher)
            : base(context, dispatcher) { }

    #endregion

    [Fact]
    public async Task Should_return_entities_from_db()
    {
        var text = Guid.NewGuid().ToString();
        var command1 = new AddOrUpdateTestEntityCommand { Text = text };
        var command2 = new AddOrUpdateTestEntityCommand { Text = text };
        var command3 = new AddOrUpdateTestEntityCommand { Text = text };
        await this.dispatcher.PushAsync(command1);
        await this.dispatcher.PushAsync(command2);
        await this.dispatcher.PushAsync(command3);

        Assert.Equal(1, command1.Result);
        Assert.Equal(2, command2.Result);
        Assert.Equal(3, command3.Result);

        var dtos = await this.dispatcher.QueryAsync(new GetTestEntitiesByIdsQuery { Ids = Array.Empty<int>() });

        Assert.Equal(3, dtos.Length);
        Assert.True(dtos.All(x => x.Text == text));
    }

    [Fact]
    public async Task Should_throw_validation_exception()
    {
        await Assert.ThrowsAsync<ValidationException>(async () => await this.dispatcher.QueryAsync(new GetTestEntitiesByIdsQuery()));
    }

    [Fact]
    public async Task Should_throw_test_exception()
    {
        await Assert.ThrowsAsync<Exception>(async () => await this.dispatcher.QueryAsync(new TestThrowingExceptionQuery()));
        await Assert.ThrowsAsync<Exception>(async () => await this.dispatcher.PushAsync(new TestThrowingExceptionCommand()));
    }

    [Fact]
    public async Task Should_rollback_changes_in_the_command()
    {
        await Assert.ThrowsAsync<Exception>(async () => await this.dispatcher.PushAsync(new TestRollbackChangesCommand()));

        Assert.Empty(await this.context.Set<TestEntity>().ToArrayAsync());
    }
}