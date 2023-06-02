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
        await SessionFactory.AddEntitiesAsync(new[] { new TestEntity { Text = Guid.NewGuid().ToString() } });

        await Repository.DeleteAsync((TestEntity)null);

        var entitiesInDb = await SessionFactory.GetEntitiesAsync<TestEntity>();

        Assert.Single(entitiesInDb);
    }

    [Fact]
    public async Task Should_delete_an_entity()
    {
        var entity = new TestEntity { Text = Guid.NewGuid().ToString() };

        await SessionFactory.AddEntitiesAsync(new[] { entity });

        await Repository.DeleteAsync(entity);

        var entitiesInDb = await SessionFactory.GetEntitiesAsync<TestEntity>();

        Assert.Empty(entitiesInDb);
    }

    [Fact]
    public async Task Should_throw_exception_for_an_not_existing_entity()
    {
        await Assert.ThrowsAsync<StaleStateException>
                (async () =>
                         await Repository.DeleteAsync(new TestEntity
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

        await SessionFactory.AddEntitiesAsync(entities.Where(r => r != null));

        await Repository.DeleteAsync(entities);

        var entitiesInDb = await SessionFactory.GetEntitiesAsync<TestEntity>();

        Assert.Empty(entitiesInDb);
    }

    [Fact]
    public async Task Should_ignore_empty_collection()
    {
        await Repository.DeleteAsync(new TestEntity[] { null });

        var entitiesInDb = await SessionFactory.GetEntitiesAsync<TestEntity>();

        Assert.Empty(entitiesInDb);
    }
}