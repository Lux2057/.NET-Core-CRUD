namespace EfTests.Core;

#region << Using >>

using CRUD.Core;
using CRUD.CQRS;

#endregion

public class CreateOrUpdateEntitiesCommandTests : ReadWriteDispatcherTest
{
    #region Constructors

    public CreateOrUpdateEntitiesCommandTests(IReadWriteDispatcher dispatcher)
            : base(dispatcher) { }

    #endregion

    [Fact]
    public async Task Should_create_and_update_entities()
    {
        var text1 = Guid.NewGuid().ToString();

        await this.dispatcher.PushAsync(new CreateOrUpdateEntitiesCommand<TestEntity, int, TestEntityDto>(new[]
                                                                                                          {
                                                                                                                  new TestEntityDto { Text = text1 },
                                                                                                                  new TestEntityDto { Text = text1 },
                                                                                                                  new TestEntityDto { Text = text1 }
                                                                                                          }));

        var dtosInDb = await this.dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, int, TestEntityDto>());

        Assert.Equal(3, dtosInDb.Length);
        Assert.True(dtosInDb.All(x => x.Text == text1));

        var text2 = Guid.NewGuid().ToString();

        Parallel.ForEach(dtosInDb, dto => dto.Text = text2);

        await this.dispatcher.PushAsync(new CreateOrUpdateEntitiesCommand<TestEntity, int, TestEntityDto>(dtosInDb));

        dtosInDb = await this.dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, int, TestEntityDto>());

        Assert.Equal(3, dtosInDb.Length);
        Assert.True(dtosInDb.All(x => x.Text == text2));

        await this.dispatcher.PushAsync(new CreateOrUpdateEntitiesCommand<TestEntity, int, TestEntityDto>(new[]
                                                                                                          {
                                                                                                                  new TestEntityDto { Text = text1 },
                                                                                                                  new TestEntityDto { Text = text1 },
                                                                                                                  new TestEntityDto { Text = text1 }
                                                                                                          }));

        dtosInDb = await this.dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, int, TestEntityDto>());

        Assert.Equal(6, dtosInDb.Length);
        Assert.Equal(3, dtosInDb.Count(x => x.Text == text1));
        Assert.Equal(3, dtosInDb.Count(x => x.Text == text2));

        await this.dispatcher.PushAsync(new CreateOrUpdateEntitiesCommand<TestEntity, int, TestEntityDto>(new[]
                                                                                                          {
                                                                                                                  new TestEntityDto { Text = text2 },
                                                                                                                  new TestEntityDto { Text = text2 },
                                                                                                                  new TestEntityDto { Text = text2 }
                                                                                                          }));

        dtosInDb = await this.dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, int, TestEntityDto>());

        Assert.Equal(9, dtosInDb.Length);
        Assert.Equal(3, dtosInDb.Count(x => x.Text == text1));
        Assert.Equal(6, dtosInDb.Count(x => x.Text == text2));
    }

    [Fact]
    public async Task Should_ignore_empty_params()
    {
        await this.dispatcher.PushAsync(new CreateOrUpdateEntitiesCommand<TestEntity, int, TestEntityDto>(Array.Empty<TestEntityDto>()));

        var dtosInDb = await this.dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, int, TestEntityDto>());

        Assert.Empty(dtosInDb);
    }
}