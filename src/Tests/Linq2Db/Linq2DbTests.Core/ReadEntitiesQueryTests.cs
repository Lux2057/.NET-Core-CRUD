namespace Linq2DbTests.Core;

#region << Using >>

using System.Collections;
using CRUD.Core;
using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using CRUD.DAL.Linq2Db;
using Extensions;
using Linq2DbTests.Shared;
using LinqToDB.Data;

#endregion

public class ReadEntitiesQueryTests : ReadDispatcherTest
{
    #region Constructors

    public ReadEntitiesQueryTests(TestDataConnection connection, IReadDispatcher dispatcher) : base(connection, dispatcher) { }

    #endregion

    [Fact]
    public async Task Should_return_entities_ordered_by_ids()
    {
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        var text = Guid.NewGuid().ToString();

        var entity1 = new TestEntity { Text = text };
        var entity2 = new TestEntity { Text = text };
        var entity3 = new TestEntity { Text = text };
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

        var orderedEntitiesIds = new[] { entity1.Id, entity2.Id, entity3.Id }.OrderBy(r => r).ToArray();

        var dtosInDb = await Dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, string, TestEntityDto>
                                                   {
                                                           OrderSpecifications = new[] { new OrderById<TestEntity, string>(false) }
                                                   });

        Assert.Equal(3, dtosInDb.Items.Length);
        Assert.True(dtosInDb.Items.All(x => x.Text == text));
        Assert.Equal(orderedEntitiesIds[0], dtosInDb.Items[0].Id);
        Assert.Equal(orderedEntitiesIds[1], dtosInDb.Items[1].Id);
        Assert.Equal(orderedEntitiesIds[2], dtosInDb.Items[2].Id);

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

        var dtosInDb = await Dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, string, TestEntityDto>(new[] { entity1.Id }));

        Assert.Single((IEnumerable)dtosInDb.Items);
        Assert.Equal(entity1.Text, dtosInDb.Items.Single().Text);

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
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        var text1 = Guid.NewGuid().ToString();
        var text2 = Guid.NewGuid().ToString();

        await Connection.BulkCopyAsync(new BulkCopyOptions
                                       {
                                               BulkCopyType = BulkCopyType.MultipleRows
                                       },
                                       new[]
                                       {
                                               new TestEntity { Text = text1 },
                                               new TestEntity { Text = text1 },
                                               new TestEntity { Text = text2 },
                                               new TestEntity { Text = text2 }
                                       });

        var dtosInDb = await Dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, string, TestEntityDto>
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

        dtosInDb = await Dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, string, TestEntityDto>
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
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        var text = Guid.NewGuid().ToString();

        await Connection.BulkCopyAsync(new BulkCopyOptions
                                       {
                                               BulkCopyType = BulkCopyType.MultipleRows
                                       },
                                       new[]
                                       {
                                               new TestEntity { Text = text },
                                               new TestEntity { Text = text },
                                               new TestEntity { Text = text },
                                               new TestEntity { Text = text },
                                               new TestEntity { Text = text },
                                               new TestEntity { Text = text }
                                       });

        var dtosInDb = await Dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, string, TestEntityDto>
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
        Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        var text1 = Guid.NewGuid().ToString();
        var text2 = Guid.NewGuid().ToString();

        await Connection.BulkCopyAsync(new BulkCopyOptions
                                       {
                                               BulkCopyType = BulkCopyType.MultipleRows
                                       },
                                       new[]
                                       {
                                               new TestEntity { Text = text1 },
                                               new TestEntity { Text = text1 },
                                               new TestEntity { Text = text2 },
                                               new TestEntity { Text = text2 }
                                       });

        var dtosInDb = await Dispatcher.QueryAsync(new ReadEntitiesQuery<TestEntity, string, TestEntityDto>
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