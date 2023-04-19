namespace EfTests.Core;

#region << Using >>

using CRUD.Core;
using CRUD.CQRS;

#endregion

public class CreateOrUpdateEntitiesCommandTests : ReadWriteDispatcherTest
{
    #region Constructors

    public CreateOrUpdateEntitiesCommandTests(TestDbContext context, IReadWriteDispatcher dispatcher)
            : base(context, dispatcher) { }

    #endregion

    [Fact]
    public async Task Should_create_entities()
    {
        var text1 = Guid.NewGuid().ToString();

        var addCommand1 = new CreateOrUpdateEntitiesCommand<TestEntity, int, TestEntityDto>(new[]
                                                                                            {
                                                                                                    new TestEntityDto { Text = text1 },
                                                                                                    new TestEntityDto { Text = text1 },
                                                                                                    new TestEntityDto { Text = text1 }
                                                                                            });

        await this.dispatcher.PushAsync(addCommand1);

        Assert.Equal(new[] { 1, 2, 3 }, addCommand1.Result);

        var dtosInDb = await this.dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, int, TestEntityDto>());

        Assert.Equal(3, dtosInDb.Length);
        Assert.True(dtosInDb.All(x => x.Text == text1));
    }

    [Fact]
    public async Task Should_update_entities()
    {
        var text1 = Guid.NewGuid().ToString();
        await this.context.Set<TestEntity>().AddRangeAsync(new[]
                                                            {
                                                                    new TestEntity { Text = text1 },
                                                                    new TestEntity { Text = text1 },
                                                                    new TestEntity { Text = text1 }
                                                            });

        await this.context.SaveChangesAsync();

        var dtosInDb = await this.dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, int, TestEntityDto>());

        Assert.Equal(3, dtosInDb.Length);
        Assert.True(dtosInDb.All(x => x.Text == text1));

        var text2 = Guid.NewGuid().ToString();

        Parallel.ForEach(dtosInDb, dto => dto.Text = text2);

        var updateCommand = new CreateOrUpdateEntitiesCommand<TestEntity, int, TestEntityDto>(dtosInDb);
        await this.dispatcher.PushAsync(updateCommand);

        Assert.Equal(new[] { 1, 2, 3 }, updateCommand.Result);

        dtosInDb = await this.dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, int, TestEntityDto>());

        Assert.Equal(3, dtosInDb.Length);
        Assert.True(dtosInDb.All(x => x.Text == text2));
    }

    [Fact]
    public async Task Should_create_and_update_entities_in_one_scope()
    {
        var text1 = Guid.NewGuid().ToString();

        var addCommand1 = new CreateOrUpdateEntitiesCommand<TestEntity, int, TestEntityDto>(new[]
                                                                                            {
                                                                                                    new TestEntityDto { Text = text1 },
                                                                                                    new TestEntityDto { Text = text1 },
                                                                                                    new TestEntityDto { Text = text1 }
                                                                                            });

        await this.dispatcher.PushAsync(addCommand1);

        Assert.Equal(new[] { 1, 2, 3 }, addCommand1.Result);

        var dtosInDb = await this.dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, int, TestEntityDto>());

        Assert.Equal(3, dtosInDb.Length);
        Assert.True(dtosInDb.All(x => x.Text == text1));

        var text2 = Guid.NewGuid().ToString();

        Parallel.ForEach(dtosInDb, dto => dto.Text = text2);

        var updateCommand = new CreateOrUpdateEntitiesCommand<TestEntity, int, TestEntityDto>(dtosInDb);
        await this.dispatcher.PushAsync(updateCommand);

        Assert.Equal(new[] { 1, 2, 3 }, updateCommand.Result);

        dtosInDb = await this.dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, int, TestEntityDto>());

        Assert.Equal(3, dtosInDb.Length);
        Assert.True(dtosInDb.All(x => x.Text == text2));

        var addCommand2 = new CreateOrUpdateEntitiesCommand<TestEntity, int, TestEntityDto>(new[]
                                                                                            {
                                                                                                    new TestEntityDto { Text = text1 },
                                                                                                    new TestEntityDto { Text = text1 },
                                                                                                    new TestEntityDto { Text = text1 }
                                                                                            });

        await this.dispatcher.PushAsync(addCommand2);

        Assert.Equal(new[] { 4, 5, 6 }, addCommand2.Result);

        dtosInDb = await this.dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, int, TestEntityDto>());

        Assert.Equal(6, dtosInDb.Length);
        Assert.Equal(3, dtosInDb.Count(x => x.Text == text1));
        Assert.Equal(3, dtosInDb.Count(x => x.Text == text2));
    }

    [Fact]
    public async Task Should_ignore_empty_params()
    {
        var command = new CreateOrUpdateEntitiesCommand<TestEntity, int, TestEntityDto>(Array.Empty<TestEntityDto>());
        await this.dispatcher.PushAsync(command);

        Assert.Null(command.Result);

        var dtosInDb = await this.dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, int, TestEntityDto>());

        Assert.Empty(dtosInDb);
    }
}