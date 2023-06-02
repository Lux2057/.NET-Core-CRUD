namespace NhTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using NHibernate;

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

        await this.repository.AddAsync(new TestEntity { Text = text });

        TestEntity[] entitiesInDb;
        using (var session = this.sessionFactory.OpenSession())
        {
            entitiesInDb = session.Query<TestEntity>().ToArray();
        }

        Assert.Single(entitiesInDb);
        Assert.Equal(text, entitiesInDb[0].Text);
    }

    [Fact]
    public async Task Should_ignore_null_entity()
    {
        await this.repository.AddAsync((TestEntity)null);

        TestEntity[] entitiesInDb;
        using (var session = this.sessionFactory.OpenSession())
        {
            entitiesInDb = session.Query<TestEntity>().ToArray();
        }

        Assert.Empty(entitiesInDb);
    }

    [Fact]
    public async Task Should_add_several_non_null_entities()
    {
        var text = Guid.NewGuid().ToString();

        await this.repository.AddAsync(new TestEntity[]
                                       {
                                               new TestEntity { Text = text },
                                               new TestEntity { Text = text },
                                               null
                                       });

        TestEntity[] entitiesInDb;
        using (var session = this.sessionFactory.OpenSession())
        {
            entitiesInDb = session.Query<TestEntity>().ToArray();
        }

        Assert.Equal(2, entitiesInDb.Length);
        Assert.True(entitiesInDb.All(r => r.Text == text));
    }

    [Fact]
    public async Task Should_ignore_empty_collection()
    {
        await this.repository.AddAsync(new[] { (TestEntity)null });

        TestEntity[] entitiesInDb;
        using (var session = this.sessionFactory.OpenSession())
        {
            entitiesInDb = session.Query<TestEntity>().ToArray();
        }

        Assert.Empty(entitiesInDb);
    }
}