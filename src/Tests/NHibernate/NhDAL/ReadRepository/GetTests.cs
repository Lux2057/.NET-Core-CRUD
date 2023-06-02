namespace NhTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using NHibernate;

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

        using (var session = this.sessionFactory.OpenSession())
        {
            await session.SaveAsync(testEntity);
            await session.FlushAsync();
        }

        Assert.Single(this.repository.Get<TestEntity>().ToArray());
        Assert.Equal(1, this.repository.Get<TestEntity>().Single().Id);
        Assert.Equal(text, this.repository.Get<TestEntity>().Single().Text);
    }

    [Fact]
    public async Task Should_return_entity_by_id()
    {
        var text = Guid.NewGuid().ToString();

        var testEntity = new TestEntity { Text = text };

        using (var session = this.sessionFactory.OpenSession())
        {
            await session.SaveAsync(testEntity);
            await session.FlushAsync();
        }

        var testEntities = this.repository.Get<TestEntity>().ToArray();
        Assert.Single(testEntities);

        var id = testEntities[0].Id;

        var entityById = this.repository.Get(new FindEntityByIntId<TestEntity>(id)).Single();

        Assert.Equal(id, entityById.Id);
        Assert.Equal(text, entityById.Text);

        entityById = this.repository.Get(new FindEntitiesByIds<TestEntity, int>(new[] { id })).Single();

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

        using (var session = this.sessionFactory.OpenSession())
        {
            foreach (var testEntity in testEntities)
                await session.SaveAsync(testEntity);

            await session.FlushAsync();
        }

        Assert.Equal(3, this.repository.Get<TestEntity>().Count());
        Assert.Equal(2, this.repository.Get(new TestByTextSpecification(text1)).Count());
        Assert.Equal(1, this.repository.Get(new TestByTextSpecification(text2)).Count());
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

        using (var session = this.sessionFactory.OpenSession())
        {
            foreach (var testEntity in testEntities)
                await session.SaveAsync(testEntity);

            await session.FlushAsync();
        }

        Assert.Equal(3, this.repository.Get(new FindEntitiesByIds<TestEntity, int>(new[] { 1, 2, 3 })).Count());
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

        using (var session = this.sessionFactory.OpenSession())
        {
            foreach (var testEntity in testEntities)
                await session.SaveAsync(testEntity);

            await session.FlushAsync();
        }

        var entitiesInDb = this.repository.Get(orderSpecifications: new[] { new OrderById<TestEntity, int>(false) }).ToArray();

        Assert.Equal(3, entitiesInDb.Length);
        Assert.Equal(1, entitiesInDb[0].Id);
        Assert.Equal(2, entitiesInDb[1].Id);
        Assert.Equal(3, entitiesInDb[2].Id);

        entitiesInDb = this.repository.Get(orderSpecifications: new[] { new OrderById<TestEntity, int>(true) }).ToArray();

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

        using (var session = this.sessionFactory.OpenSession())
        {
            foreach (var testEntity in testEntities)
                await session.SaveAsync(testEntity);

            await session.FlushAsync();
        }

        var entitiesInDb = this.repository.Get(orderSpecifications: new OrderSpecification<TestEntity>[]
                                                                    {
                                                                            new OrderByTextTestSpecification(true),
                                                                            new OrderById<TestEntity, int>(false)
                                                                    }).ToArray();

        Assert.Equal(3, entitiesInDb.Length);
        Assert.Equal(2, entitiesInDb[0].Id);
        Assert.Equal(3, entitiesInDb[1].Id);
        Assert.Equal(1, entitiesInDb[2].Id);

        entitiesInDb = this.repository.Get(orderSpecifications: new OrderSpecification<TestEntity>[]
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