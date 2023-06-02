namespace NhTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using NHibernate;

#endregion

public class DeleteAsyncTests : NhRepositoryTest
{
    #region Constructors

    public DeleteAsyncTests(ISessionFactory sessionFactory, IRepository repository)
            : base(sessionFactory, repository) { }

    #endregion

    [Fact]
    public async Task Should_ignore_null_entity()
    {
        using (var session = this.sessionFactory.OpenSession())
        {
            await session.SaveAsync(new TestEntity { Text = Guid.NewGuid().ToString() });
            await session.FlushAsync();
        }

        await this.repository.DeleteAsync((TestEntity)null);

        TestEntity[] entitiesInDb;
        using (var session = this.sessionFactory.OpenSession())
        {
            entitiesInDb = session.Query<TestEntity>().ToArray();
        }

        Assert.Single(entitiesInDb);
    }

    [Fact]
    public async Task Should_delete_an_entity()
    {
        var entity = new TestEntity { Text = Guid.NewGuid().ToString() };

        using (var session = this.sessionFactory.OpenSession())
        {
            await session.SaveAsync(entity);
            await session.FlushAsync();
        }

        await this.repository.DeleteAsync(entity);

        TestEntity[] entitiesInDb;
        using (var session = this.sessionFactory.OpenSession())
        {
            entitiesInDb = session.Query<TestEntity>().ToArray();
        }

        Assert.Empty(entitiesInDb);
    }

    [Fact]
    public async Task Should_throw_exception_for_an_not_existing_entity()
    {
        await Assert.ThrowsAsync<StaleStateException>
                (async () =>
                         await this.repository.DeleteAsync(new TestEntity
                                                           {
                                                                   Id = 1,
                                                                   Text = Guid.NewGuid().ToString()
                                                           }));
    }

    [Fact]
    public async Task Should_delete_several_non_null_entities()
    {
        var entities = new[]
                       {
                               new TestEntity { Text = Guid.NewGuid().ToString() },
                               new TestEntity { Text = Guid.NewGuid().ToString() },
                               null
                       };

        using (var session = this.sessionFactory.OpenSession())
        {
            foreach (var entity in entities.Where(r => r != null))
                await session.SaveAsync(entity);

            await session.FlushAsync();
        }

        await this.repository.DeleteAsync(entities);

        TestEntity[] entitiesInDb;
        using (var session = this.sessionFactory.OpenSession())
        {
            entitiesInDb = session.Query<TestEntity>().ToArray();
        }

        Assert.Empty(entitiesInDb);
    }

    [Fact]
    public async Task Should_ignore_empty_collection()
    {
        await this.repository.DeleteAsync(new TestEntity[] { null });

        TestEntity[] entitiesInDb;
        using (var session = this.sessionFactory.OpenSession())
        {
            entitiesInDb = session.Query<TestEntity>().ToArray();
        }

        Assert.Empty(entitiesInDb);
    }
}