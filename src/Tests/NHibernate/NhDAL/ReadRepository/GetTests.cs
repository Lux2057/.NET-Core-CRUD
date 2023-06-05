namespace NhTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using NHibernate;
using NhTests.Shared;

#endregion

public class GetTests : NhReadRepositoryTest
{
    #region Constructors

    public GetTests(IReadRepository repository, ISessionFactory sessionFactory)
            : base(repository, sessionFactory) { }

    #endregion

    [Fact]
    public async Task Should_return_entity()
    {
        var text = Guid.NewGuid().ToString();

        var testEntity = new TestEntity { Text = text };

        await SessionFactory.AddEntitiesAsync(new[] { testEntity });

        var entities = Repository.Get<TestEntity>().ToArray();

        Assert.Single(entities);
        Assert.Equal(testEntity.Id, entities.Single().Id);
        Assert.Equal(text, entities.Single().Text);
    }

    [Fact]
    public async Task Should_return_entity_by_id()
    {
        var text = Guid.NewGuid().ToString();

        var testEntity = new TestEntity { Text = text };

        await SessionFactory.AddEntitiesAsync(new[] { testEntity });

        var testEntities = Repository.Get<TestEntity>().ToArray();
        Assert.Single(testEntities);

        var id = testEntities[0].Id;

        var entityById = Repository.Get(new FindEntityByIntId<TestEntity>(id)).Single();

        Assert.Equal(id, entityById.Id);
        Assert.Equal(text, entityById.Text);

        entityById = Repository.Get(new FindEntitiesByIds<TestEntity, int>(new[] { id })).Single();

        Assert.Equal(id, entityById.Id);
        Assert.Equal(text, entityById.Text);
    }

    [Fact]
    public async Task Should_return_entities_by_text_spec()
    {
        var text1 = Guid.NewGuid().ToString();
        var text2 = Guid.NewGuid().ToString();

        var testEntities = new[]
                           {
                                   new TestEntity { Text = text1 },
                                   new TestEntity { Text = text1 },
                                   new TestEntity { Text = text2 }
                           };

        await SessionFactory.AddEntitiesAsync(testEntities);

        Assert.Equal(3, Repository.Get<TestEntity>().Count());
        Assert.Equal(2, Repository.Get(new TestByTextSpecification(text1)).Count());
        Assert.Equal(1, Repository.Get(new TestByTextSpecification(text2)).Count());
    }

    [Fact]
    public async Task Should_return_entities_by_ids_spec()
    {
        var text1 = Guid.NewGuid().ToString();
        var text2 = Guid.NewGuid().ToString();

        var testEntities = new[]
                           {
                                   new TestEntity { Text = text1 },
                                   new TestEntity { Text = text1 },
                                   new TestEntity { Text = text2 }
                           };

        await SessionFactory.AddEntitiesAsync(testEntities);

        Assert.Equal(3, Repository.Get(new FindEntitiesByIds<TestEntity, int>(new[] { 1, 2, 3 })).Count());
    }

    [Fact]
    public async Task Should_return_entities_ordered_by_ids()
    {
        var text = Guid.NewGuid().ToString();

        var testEntities = new[]
                           {
                                   new TestEntity { Text = text },
                                   new TestEntity { Text = text },
                                   new TestEntity { Text = text }
                           };

        await SessionFactory.AddEntitiesAsync(testEntities);

        var entitiesInDb = Repository.Get(orderSpecifications: new[] { new OrderById<TestEntity, int>(false) }).ToArray();

        Assert.Equal(3, entitiesInDb.Length);
        Assert.Equal(1, entitiesInDb[0].Id);
        Assert.Equal(2, entitiesInDb[1].Id);
        Assert.Equal(3, entitiesInDb[2].Id);

        entitiesInDb = Repository.Get(orderSpecifications: new[] { new OrderById<TestEntity, int>(true) }).ToArray();

        Assert.Equal(3, entitiesInDb.Length);
        Assert.Equal(1, entitiesInDb[2].Id);
        Assert.Equal(2, entitiesInDb[1].Id);
        Assert.Equal(3, entitiesInDb[0].Id);
    }

    [Fact]
    public async Task Should_return_entities_ordered_by_text_then_by_ids()
    {
        var testEntities = new[]
                           {
                                   new TestEntity { Text = 1.ToString() },
                                   new TestEntity { Text = 2.ToString() },
                                   new TestEntity { Text = 2.ToString() }
                           };

        await SessionFactory.AddEntitiesAsync(testEntities);

        var entitiesInDb = Repository.Get(orderSpecifications: new OrderSpecification<TestEntity>[]
                                                               {
                                                                       new OrderByTextTestSpecification(true),
                                                                       new OrderById<TestEntity, int>(false)
                                                               }).ToArray();

        Assert.Equal(3, entitiesInDb.Length);
        Assert.Equal(2, entitiesInDb[0].Id);
        Assert.Equal(3, entitiesInDb[1].Id);
        Assert.Equal(1, entitiesInDb[2].Id);

        entitiesInDb = Repository.Get(orderSpecifications: new OrderSpecification<TestEntity>[]
                                                           {
                                                                   new OrderByTextTestSpecification(true),
                                                                   new OrderById<TestEntity, int>(true)
                                                           }).ToArray();

        Assert.Equal(3, entitiesInDb.Length);
        Assert.Equal(3, entitiesInDb[0].Id);
        Assert.Equal(2, entitiesInDb[1].Id);
        Assert.Equal(1, entitiesInDb[2].Id);
    }
}