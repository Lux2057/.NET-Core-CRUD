namespace NhTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using NHibernate;
using NhTests.Shared;

#endregion

public class AddAsyncTests : NhRepositoryTest
{
    #region Constructors

    public AddAsyncTests(ISessionFactory sessionFactory, IRepository repository)
            : base(sessionFactory, repository) { }

    #endregion

    [Fact]
    public async Task Should_add_single_entity()
    {
        var text = Guid.NewGuid().ToString();

        await Repository.AddAsync(new TestEntity { Text = text });

        var entitiesInDb = await SessionFactory.GetEntitiesAsync<TestEntity>();

        Assert.Single(entitiesInDb);
        Assert.Equal(text, entitiesInDb[0].Text);
    }

    [Fact]
    public async Task Should_ignore_null_entity()
    {
        await Repository.AddAsync((TestEntity)null);

        var entitiesInDb = await SessionFactory.GetEntitiesAsync<TestEntity>();

        Assert.Empty(entitiesInDb);
    }

    [Fact]
    public async Task Should_add_several_non_null_entities()
    {
        var text = Guid.NewGuid().ToString();

        await Repository.AddAsync(new TestEntity[]
                                  {
                                          new TestEntity { Text = text },
                                          new TestEntity { Text = text },
                                          null
                                  });

        var entitiesInDb = await SessionFactory.GetEntitiesAsync<TestEntity>();

        Assert.Equal(2, entitiesInDb.Length);
        Assert.True(entitiesInDb.All(r => r.Text == text));
    }

    [Fact]
    public async Task Should_ignore_empty_collection()
    {
        await Repository.AddAsync(new[] { (TestEntity)null });

        var entitiesInDb = await SessionFactory.GetEntitiesAsync<TestEntity>();

        Assert.Empty(entitiesInDb);
    }
}