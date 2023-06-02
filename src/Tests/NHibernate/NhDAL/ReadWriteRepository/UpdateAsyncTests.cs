namespace NhTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using NHibernate;

#endregion

public class UpdateAsyncTests : NhRepositoryTest
{
    #region Constructors

    public UpdateAsyncTests(ISessionFactory sessionFactory, IRepository repository)
            : base(sessionFactory, repository) { }

    #endregion

    [Fact]
    public async Task Should_update_an_entity()
    {
        var oldText = Guid.NewGuid().ToString();
        var newText = Guid.NewGuid().ToString();

        var entity = new TestEntity { Text = oldText };

        using (var session = this.sessionFactory.OpenSession())
        {
            await session.SaveAsync(entity);
            await session.FlushAsync();
        }

        entity.Text = newText;
        await this.repository.UpdateAsync(entity);

        TestEntity[] entitiesInDb;
        using (var session = this.sessionFactory.OpenSession())
        {
            entitiesInDb = session.Query<TestEntity>().ToArray();
        }

        Assert.Single(entitiesInDb);
        Assert.Equal(newText, entitiesInDb[0].Text);
    }

    [Fact]
    public async Task Should_ignore_null()
    {
        await this.repository.UpdateAsync((TestEntity)null);

        TestEntity[] entitiesInDb;
        using (var session = this.sessionFactory.OpenSession())
        {
            entitiesInDb = session.Query<TestEntity>().ToArray();
        }

        Assert.Empty(entitiesInDb);
    }

    [Fact]
    public async Task Should_throw_exception_for_nonexistent_entity()
    {
        await Assert.ThrowsAsync<StaleObjectStateException>
                (async () => await this.repository
                                       .UpdateAsync(new TestEntity
                                                    {
                                                            Id = 1,
                                                            Text = Guid.NewGuid().ToString()
                                                    }));
    }

    [Fact]
    public async Task Should_update_several_non_null_entities()
    {
        var oldText = Guid.NewGuid().ToString();
        var newText = Guid.NewGuid().ToString();

        var firstEntity = new TestEntity { Text = oldText };
        var entities = new[]
                       {
                               firstEntity,
                               new TestEntity { Text = oldText },
                               null
                       };

        using (var session = this.sessionFactory.OpenSession())
        {
            foreach (var entity in entities.Where(r => r != null))
                await session.SaveAsync(entity);

            await session.FlushAsync();
        }

        firstEntity.Text = newText;
        await this.repository.UpdateAsync(entities);

        TestEntity[] entitiesInDb;
        using (var session = this.sessionFactory.OpenSession())
        {
            entitiesInDb = session.Query<TestEntity>().ToArray();
        }

        Assert.Equal(2, entitiesInDb.Length);
        Assert.True(entitiesInDb.Count(r => r.Text == oldText) == 1);
        Assert.True(entitiesInDb.Count(r => r.Text == newText) == 1);
    }

    [Fact]
    public async Task Should_ignore_empty_collection()
    {
        await this.repository.UpdateAsync(new TestEntity[] { null });

        TestEntity[] entitiesInDb;
        using (var session = this.sessionFactory.OpenSession())
        {
            entitiesInDb = session.Query<TestEntity>().ToArray();
        }

        Assert.Empty(entitiesInDb);
    }

    [Fact]
    public async Task Should_be_updated_explicitly_only()
    {
        var oldText = Guid.NewGuid().ToString();
        var newText = Guid.NewGuid().ToString();

        var entities = new[]
                       {
                               new TestEntity { Text = oldText },
                               new TestEntity { Text = oldText }
                       };

        await this.repository.AddAsync(entities);

        var entitiesInDb = this.repository.Get<TestEntity>().ToArray();

        foreach (var entity in entitiesInDb)
            entity.Text = newText;

        entitiesInDb = this.repository.Get<TestEntity>().ToArray();

        Assert.True(entitiesInDb.All(x => x.Text == oldText));
    }
}