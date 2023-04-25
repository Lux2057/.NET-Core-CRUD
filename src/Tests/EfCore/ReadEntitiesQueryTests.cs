namespace EfTests.Core;

#region << Using >>

using CRUD.Core;
using CRUD.CQRS;

#endregion

public class ReadEntitiesQueryTests : ReadDispatcherTest
{
    #region Constructors

    public ReadEntitiesQueryTests(IReadDispatcher dispatcher, TestDbContext context)
            : base(dispatcher, context) { }

    #endregion

    [Fact]
    public async Task Should_return_entities()
    {
        var text = Guid.NewGuid().ToString();

        await this.context.Set<TestEntity>().AddRangeAsync(new[]
                                                           {
                                                                   new TestEntity { Text = text },
                                                                   new TestEntity { Text = text },
                                                                   new TestEntity { Text = text }
                                                           });

        await this.context.SaveChangesAsync();

        var dtosInDb = await this.dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, int, TestEntityDto>());

        Assert.Equal(3, dtosInDb.Length);
        Assert.True(dtosInDb.All(x => x.Text == text));
    }

    [Fact]
    public async Task Should_return_entities_by_ids()
    {
        var text = Guid.NewGuid().ToString();

        await this.context.Set<TestEntity>().AddRangeAsync(new[]
                                                           {
                                                                   new TestEntity { Text = text },
                                                                   new TestEntity { Text = text },
                                                                   new TestEntity { Text = text }
                                                           });

        await this.context.SaveChangesAsync();

        var dtosInDb = await this.dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, int, TestEntityDto>(new[] { 1 }));

        Assert.Single(dtosInDb);
        Assert.Equal(text, dtosInDb.Single().Text);
    }

    [Fact]
    public async Task Should_return_entities_page()
    {
        var text1 = Guid.NewGuid().ToString();
        var text2 = Guid.NewGuid().ToString();

        await this.context.Set<TestEntity>().AddRangeAsync(new[]
                                                           {
                                                                   new TestEntity { Text = text1 },
                                                                   new TestEntity { Text = text1 },
                                                                   new TestEntity { Text = text2 },
                                                                   new TestEntity { Text = text2 }
                                                           });

        await this.context.SaveChangesAsync();

        var dtosInDb = await this.dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, int, TestEntityDto>
                                                        {
                                                                Page = 1,
                                                                PageSize = 2
                                                        });

        Assert.Equal(2, dtosInDb.Length);
        Assert.True(dtosInDb.All(x => x.Text == text1));

        dtosInDb = await this.dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, int, TestEntityDto>
                                                    {
                                                            Page = 2,
                                                            PageSize = 2
                                                    });

        Assert.Equal(2, dtosInDb.Length);
        Assert.True(dtosInDb.All(x => x.Text == text2));
    }

    [Fact]
    public async Task Should_return_all_entities()
    {
        var text = Guid.NewGuid().ToString();

        await this.context.Set<TestEntity>().AddRangeAsync(new[]
                                                           {
                                                                   new TestEntity { Text = text },
                                                                   new TestEntity { Text = text },
                                                                   new TestEntity { Text = text },
                                                                   new TestEntity { Text = text },
                                                                   new TestEntity { Text = text },
                                                                   new TestEntity { Text = text }
                                                           });

        await this.context.SaveChangesAsync();

        var dtosInDb = await this.dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, int, TestEntityDto>
                                                        {
                                                                Page = 1,
                                                                PageSize = 2,
                                                                DisablePaging = true
                                                        });

        Assert.Equal(6, dtosInDb.Length);
        Assert.True(dtosInDb.All(x => x.Text == text));
    }

    [Fact]
    public async Task Should_return_entities_by_spec()
    {
        var text1 = Guid.NewGuid().ToString();
        var text2 = Guid.NewGuid().ToString();

        await this.context.Set<TestEntity>().AddRangeAsync(new[]
                                                           {
                                                                   new TestEntity { Text = text1 },
                                                                   new TestEntity { Text = text1 },
                                                                   new TestEntity { Text = text2 },
                                                                   new TestEntity { Text = text2 }
                                                           });

        await this.context.SaveChangesAsync();

        var dtosInDb = await this.dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, int, TestEntityDto>
                                                        {
                                                                DisablePaging = true,
                                                                Specification = new TestEntityByTextSpec(text1)
                                                        });

        Assert.Equal(2, dtosInDb.Length);
        Assert.True(dtosInDb.All(x => x.Text == text1));
    }
}