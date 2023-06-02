namespace EfTests.Core;

#region << Using >>

using CRUD.Core;
using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using CRUD.Extensions;

#endregion

public class ReadEntitiesQueryTests : ReadDispatcherTest
{
    #region Constructors

    public ReadEntitiesQueryTests(IReadDispatcher dispatcher, TestDbContext context)
            : base(dispatcher, context) { }

    #endregion

    [Fact]
    public async Task Should_return_entities_ordered_by_ids()
    {
        var text = Guid.NewGuid().ToString();

        await this.context.Set<TestEntity>().AddRangeAsync(new[]
                                                           {
                                                                   new TestEntity { Text = text },
                                                                   new TestEntity { Text = text },
                                                                   new TestEntity { Text = text }
                                                           });

        await this.context.SaveChangesAsync();

        var dtosInDb = await this.dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, int, TestEntityDto>
                                                        {
                                                                OrderSpecifications = new[] { new OrderById<TestEntity, int>(false) }
                                                        });

        Assert.Equal(3, dtosInDb.Items.Length);
        Assert.True(dtosInDb.Items.All(x => x.Text == text));
        Assert.Equal(1, dtosInDb.Items[0].Id);
        Assert.Equal(2, dtosInDb.Items[1].Id);
        Assert.Equal(3, dtosInDb.Items[2].Id);

        Assert.Equal(new PagingInfoDto
                     {
                             CurrentPage = GetPagingInfoQuery.defaultPage,
                             PageSize = GetPagingInfoQuery.defaultPageSize,
                             TotalItemsCount = 3,
                             TotalPages = 1
                     }.ToJsonString(),
                     dtosInDb.PagingInfo.ToJsonString());
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

        Assert.Single(dtosInDb.Items);
        Assert.Equal(text, dtosInDb.Items.Single().Text);

        Assert.Equal(new PagingInfoDto
                     {
                             CurrentPage = GetPagingInfoQuery.defaultPage,
                             PageSize = GetPagingInfoQuery.defaultPageSize,
                             TotalItemsCount = 1,
                             TotalPages = 1
                     }.ToJsonString(),
                     dtosInDb.PagingInfo.ToJsonString());
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

        Assert.Equal(2, dtosInDb.Items.Length);
        Assert.True(dtosInDb.Items.All(x => x.Text == text1));

        Assert.Equal(new PagingInfoDto
                     {
                             CurrentPage = 1,
                             PageSize = 2,
                             TotalItemsCount = 4,
                             TotalPages = 2
                     }.ToJsonString(),
                     dtosInDb.PagingInfo.ToJsonString());

        dtosInDb = await this.dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, int, TestEntityDto>
                                                    {
                                                            Page = 2,
                                                            PageSize = 2
                                                    });

        Assert.Equal(2, dtosInDb.Items.Length);
        Assert.True(dtosInDb.Items.All(x => x.Text == text2));

        Assert.Equal(new PagingInfoDto
                     {
                             CurrentPage = 2,
                             PageSize = 2,
                             TotalItemsCount = 4,
                             TotalPages = 2
                     }.ToJsonString(),
                     dtosInDb.PagingInfo.ToJsonString());
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

        Assert.Equal(6, dtosInDb.Items.Length);
        Assert.True(dtosInDb.Items.All(x => x.Text == text));

        Assert.Equal(new PagingInfoDto
                     {
                             CurrentPage = 1,
                             PageSize = 6,
                             TotalItemsCount = 6,
                             TotalPages = 1
                     }.ToJsonString(),
                     dtosInDb.PagingInfo.ToJsonString());
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

        Assert.Equal(2, dtosInDb.Items.Length);
        Assert.True(dtosInDb.Items.All(x => x.Text == text1));

        Assert.Equal(new PagingInfoDto
                     {
                             CurrentPage = 1,
                             PageSize = 2,
                             TotalItemsCount = 2,
                             TotalPages = 1
                     }.ToJsonString(),
                     dtosInDb.PagingInfo.ToJsonString());
    }
}