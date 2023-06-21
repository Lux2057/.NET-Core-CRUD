namespace EfTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using EfTests.Shared;
using Microsoft.EntityFrameworkCore;

#endregion

public class ReadTests : EfReadRepositoryTest
{
    #region Constructors

    public ReadTests(TestDbContext context, IReadRepository repository)
            : base(context, repository) { }

    #endregion

    [Fact]
    public async Task Should_return_entity()
    {
        var text = Guid.NewGuid().ToString();

        var testEntity = new TestEntity { Text = text };

        await this.context.Set<TestEntity>().AddAsync(testEntity);
        await this.context.SaveChangesAsync();

        Assert.Single(this.repository.Read<TestEntity>().ToArray());
        Assert.Equal(1, this.repository.Read<TestEntity>().Single().Id);
        Assert.Equal(text, this.repository.Read<TestEntity>().Single().Text);
    }

    [Fact]
    public async Task Should_return_entity_by_id()
    {
        var text = Guid.NewGuid().ToString();

        var testEntity = new TestEntity { Text = text };

        await this.context.Set<TestEntity>().AddAsync(testEntity);
        await this.context.SaveChangesAsync();

        Assert.Single(this.repository.Read<TestEntity>().ToArray());
        Assert.Equal(1, this.repository.Read(new FindEntityByIntId<TestEntity>(1)).Single().Id);
        Assert.Equal(1, this.repository.Read(new FindEntitiesByIds<TestEntity, int>(new[] { 1 })).Single().Id);
        Assert.Equal(text, this.repository.Read<TestEntity>().Single().Text);
    }

    [Fact]
    public async Task Should_return_entities_by_text_spec()
    {
        var text1 = Guid.NewGuid().ToString();
        var text2 = Guid.NewGuid().ToString();

        await this.context.Set<TestEntity>().AddRangeAsync(new TestEntity
                                                           {
                                                                   Text = text1
                                                           },
                                                           new TestEntity
                                                           {
                                                                   Text = text1
                                                           },
                                                           new TestEntity
                                                           {
                                                                   Text = text2
                                                           });

        await this.context.SaveChangesAsync();

        Assert.Equal(3, this.repository.Read<TestEntity>().Count());
        Assert.Equal(2, this.repository.Read(new TestByTextSpecification(text1)).Count());
        Assert.Equal(1, this.repository.Read(new TestByTextSpecification(text2)).Count());
    }

    [Fact]
    public async Task Should_return_entities_by_ids_spec()
    {
        var text1 = Guid.NewGuid().ToString();
        var text2 = Guid.NewGuid().ToString();

        await this.context.Set<TestEntity>().AddRangeAsync(new TestEntity
                                                           {
                                                                   Text = text1
                                                           },
                                                           new TestEntity
                                                           {
                                                                   Text = text1
                                                           },
                                                           new TestEntity
                                                           {
                                                                   Text = text2
                                                           });

        await this.context.SaveChangesAsync();

        Assert.Equal(3, this.repository.Read(new FindEntitiesByIds<TestEntity, int>(new[] { 1, 2, 3 })).Count());
    }

    [Fact]
    public async Task Should_return_entities_ordered_by_ids()
    {
        var text = Guid.NewGuid().ToString();

        await this.context.Set<TestEntity>().AddRangeAsync(new TestEntity { Text = text },
                                                           new TestEntity { Text = text },
                                                           new TestEntity { Text = text });

        await this.context.SaveChangesAsync();

        var entitiesInDb = await this.repository.Read(orderSpecifications: new[] { new OrderById<TestEntity, int>(false) }).ToArrayAsync();

        Assert.Equal(3, entitiesInDb.Length);
        Assert.Equal(1, entitiesInDb[0].Id);
        Assert.Equal(2, entitiesInDb[1].Id);
        Assert.Equal(3, entitiesInDb[2].Id);

        entitiesInDb = await this.repository.Read(orderSpecifications: new[] { new OrderById<TestEntity, int>(true) }).ToArrayAsync();

        Assert.Equal(3, entitiesInDb.Length);
        Assert.Equal(1, entitiesInDb[2].Id);
        Assert.Equal(2, entitiesInDb[1].Id);
        Assert.Equal(3, entitiesInDb[0].Id);
    }

    [Fact]
    public async Task Should_return_entities_ordered_by_text_then_by_ids()
    {
        await this.context.Set<TestEntity>().AddRangeAsync(new TestEntity { Text = 1.ToString() },
                                                           new TestEntity { Text = 2.ToString() },
                                                           new TestEntity { Text = 2.ToString() });

        await this.context.SaveChangesAsync();

        var entitiesInDb = await this.repository.Read(orderSpecifications: new OrderSpecification<TestEntity>[]
                                                                          {
                                                                                  new OrderByTextTestSpecification(true),
                                                                                  new OrderById<TestEntity, int>(false)
                                                                          }).ToArrayAsync();

        Assert.Equal(3, entitiesInDb.Length);
        Assert.Equal(2, entitiesInDb[0].Id);
        Assert.Equal(3, entitiesInDb[1].Id);
        Assert.Equal(1, entitiesInDb[2].Id);

        entitiesInDb = await this.repository.Read(orderSpecifications: new OrderSpecification<TestEntity>[]
                                                                      {
                                                                              new OrderByTextTestSpecification(true),
                                                                              new OrderById<TestEntity, int>(true)
                                                                      }).ToArrayAsync();

        Assert.Equal(3, entitiesInDb.Length);
        Assert.Equal(3, entitiesInDb[0].Id);
        Assert.Equal(2, entitiesInDb[1].Id);
        Assert.Equal(1, entitiesInDb[2].Id);
    }
}