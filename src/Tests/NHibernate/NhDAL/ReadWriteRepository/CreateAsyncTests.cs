namespace NhTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using NHibernate;
using NhTests.Shared;

#endregion

public class CreateAsyncTests : NhRepositoryTest
{
    #region Constructors

    public CreateAsyncTests(ISessionFactory sessionFactory, IRepository repository)
            : base(sessionFactory, repository) { }

    #endregion

    [Fact]
    public async Task Should_create_single_entity()
    {
        var text = Guid.NewGuid().ToString();

        await Repository.CreateAsync(new TestEntity { Text = text });

        var entitiesInDb = await SessionFactory.GetEntitiesAsync<TestEntity>();

        Assert.Single(entitiesInDb);
        Assert.Equal(text, entitiesInDb[0].Text);
    }

    [Fact]
    public async Task Should_ignore_null_entity()
    {
        await Repository.CreateAsync((TestEntity)null);

        var entitiesInDb = await SessionFactory.GetEntitiesAsync<TestEntity>();

        Assert.Empty(entitiesInDb);
    }

    [Fact]
    public async Task Should_create_several_non_null_entities()
    {
        var text = Guid.NewGuid().ToString();

        await Repository.CreateAsync(new TestEntity[]
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
        await Repository.CreateAsync(new[] { (TestEntity)null });

        var entitiesInDb = await SessionFactory.GetEntitiesAsync<TestEntity>();

        Assert.Empty(entitiesInDb);
    }
}