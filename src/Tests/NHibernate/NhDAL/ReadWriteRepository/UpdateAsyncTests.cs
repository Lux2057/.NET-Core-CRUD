namespace NhTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using NHibernate;
using NhTests.Shared;

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

        await SessionFactory.AddEntitiesAsync(new[] { entity });

        entity.Text = newText;
        await this.Repository.UpdateAsync(entity);

        var entitiesInDb = await SessionFactory.GetEntitiesAsync<TestEntity>();

        Assert.Single(entitiesInDb);
        Assert.Equal(newText, entitiesInDb[0].Text);
    }

    [Fact]
    public async Task Should_ignore_null()
    {
        await this.Repository.UpdateAsync((TestEntity)null);

        var entitiesInDb = await SessionFactory.GetEntitiesAsync<TestEntity>();

        Assert.Empty(entitiesInDb);
    }

    [Fact]
    public async Task Should_throw_exception_for_nonexistent_entity()
    {
        await Assert.ThrowsAsync<StaleObjectStateException>
                (async () => await this.Repository
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

        await SessionFactory.AddEntitiesAsync(entities.Where(r => r != null));

        firstEntity.Text = newText;
        await this.Repository.UpdateAsync(entities);

        var entitiesInDb = await SessionFactory.GetEntitiesAsync<TestEntity>();

        Assert.Equal(2, entitiesInDb.Length);
        Assert.True(entitiesInDb.Count(r => r.Text == oldText) == 1);
        Assert.True(entitiesInDb.Count(r => r.Text == newText) == 1);
    }

    [Fact]
    public async Task Should_ignore_empty_collection()
    {
        await this.Repository.UpdateAsync(new TestEntity[] { null });

        var entitiesInDb = await SessionFactory.GetEntitiesAsync<TestEntity>();

        Assert.Empty(entitiesInDb);
    }

    [Fact]
    public async Task Should_be_updated_explicitly_only()
    {
        const string oldText = "old text";
        const string newText = "new text";

        var entities = new[]
                       {
                               new TestEntity { Text = oldText },
                               new TestEntity { Text = oldText }
                       };

        await this.Repository.AddAsync(entities);

        foreach (var entity in entities)
            entity.Text = newText;

        entities = this.Repository.Get<TestEntity>().ToArray();

        Assert.True(entities.All(x => x.Text == oldText));
    }
}