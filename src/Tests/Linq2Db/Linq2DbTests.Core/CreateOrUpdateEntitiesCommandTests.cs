namespace Linq2DbTests.Core;

#region << Using >>

using CRUD.Core;
using CRUD.CQRS;
using CRUD.DAL.Linq2Db;
using Linq2DbTests.Shared;
using LinqToDB.Data;

#endregion

public class CreateOrUpdateEntitiesCommandTests : DispatcherTest
{
    #region Constructors

    public CreateOrUpdateEntitiesCommandTests(TestDataConnection connection, IDispatcher dispatcher) : base(connection, dispatcher) { }

    #endregion

    [Fact]
    public async Task Should_create_entities()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        var text1 = Guid.NewGuid().ToString();

        var addCommand1 = new CreateOrUpdateEntitiesCommand<TestEntity, string, TestEntityDto>(new[]
                                                                                               {
                                                                                                       new TestEntityDto { Text = text1 },
                                                                                                       new TestEntityDto { Text = text1 },
                                                                                                       new TestEntityDto { Text = text1 }
                                                                                               });

        await Dispatcher.PushAsync(addCommand1);

        Assert.Equal(3, addCommand1.Result.Length);

        var dtosInDb = await Dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, string, TestEntityDto>());

        Assert.Equal(3, dtosInDb.Items.Length);
        Assert.True(dtosInDb.Items.All(x => x.Text == text1));
    }

    [Fact]
    public async Task Should_update_entities()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        var text1 = Guid.NewGuid().ToString();

        await Connection.BulkCopyAsync(new BulkCopyOptions
                                       {
                                               BulkCopyType = BulkCopyType.MultipleRows
                                       },
                                       new[]
                                       {
                                               new TestEntity { Text = text1 },
                                               new TestEntity { Text = text1 },
                                               new TestEntity { Text = text1 }
                                       });

        var dtosInDb = await Dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, string, TestEntityDto>());

        Assert.Equal(3, dtosInDb.Items.Length);
        Assert.True(dtosInDb.Items.All(x => x.Text == text1));

        var text2 = Guid.NewGuid().ToString();

        Parallel.ForEach(dtosInDb.Items, dto => dto.Text = text2);

        var updateCommand = new CreateOrUpdateEntitiesCommand<TestEntity, string, TestEntityDto>(dtosInDb.Items);
        await Dispatcher.PushAsync(updateCommand);

        Assert.Equal(3, updateCommand.Result.Length);

        dtosInDb = await Dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, string, TestEntityDto>());

        Assert.Equal(3, dtosInDb.Items.Length);
        Assert.True(dtosInDb.Items.All(x => x.Text == text2));
    }

    [Fact]
    public async Task Should_create_and_update_entities_in_one_scope()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        var text1 = Guid.NewGuid().ToString();

        var addCommand1 = new CreateOrUpdateEntitiesCommand<TestEntity, string, TestEntityDto>(new[]
                                                                                               {
                                                                                                       new TestEntityDto { Text = text1 },
                                                                                                       new TestEntityDto { Text = text1 },
                                                                                                       new TestEntityDto { Text = text1 }
                                                                                               });

        await Dispatcher.PushAsync(addCommand1);

        Assert.Equal(3, addCommand1.Result.Length);

        var dtosInDb = await Dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, string, TestEntityDto>());

        Assert.Equal(3, dtosInDb.Items.Length);
        Assert.True(dtosInDb.Items.All(x => x.Text == text1));

        var text2 = Guid.NewGuid().ToString();

        Parallel.ForEach(dtosInDb.Items, dto => dto.Text = text2);

        var updateCommand = new CreateOrUpdateEntitiesCommand<TestEntity, string, TestEntityDto>(dtosInDb.Items);
        await Dispatcher.PushAsync(updateCommand);

        Assert.Equal(3, updateCommand.Result.Length);

        dtosInDb = await Dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, string, TestEntityDto>());

        Assert.Equal(3, dtosInDb.Items.Length);
        Assert.True(dtosInDb.Items.All(x => x.Text == text2));

        var addCommand2 = new CreateOrUpdateEntitiesCommand<TestEntity, string, TestEntityDto>(new[]
                                                                                               {
                                                                                                       new TestEntityDto { Text = text1 },
                                                                                                       new TestEntityDto { Text = text1 },
                                                                                                       new TestEntityDto { Text = text1 }
                                                                                               });

        await Dispatcher.PushAsync(addCommand2);

        Assert.Equal(3, addCommand2.Result.Length);

        dtosInDb = await Dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, string, TestEntityDto>());

        Assert.Equal(6, dtosInDb.Items.Length);
        Assert.Equal(3, dtosInDb.Items.Count(x => x.Text == text1));
        Assert.Equal(3, dtosInDb.Items.Count(x => x.Text == text2));
    }

    [Fact]
    public async Task Should_ignore_empty_params()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        var command = new CreateOrUpdateEntitiesCommand<TestEntity, string, TestEntityDto>(Array.Empty<TestEntityDto>());
        await Dispatcher.PushAsync(command);

        Assert.Null(command.Result);

        var dtosInDb = await Dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, string, TestEntityDto>());

        Assert.Empty(dtosInDb.Items);
    }
}