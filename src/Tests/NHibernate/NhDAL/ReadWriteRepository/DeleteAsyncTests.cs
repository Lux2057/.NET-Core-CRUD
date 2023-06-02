namespace NhTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using NHibernate;

#endregion

public class DeleteAsyncTests : NhRepositoryTest
{
    #region Constructors

    public DeleteAsyncTests(ISession session, IRepository repository)
            : base(session, repository) { }

    #endregion

    [Fact]
    public async Task Should_ignore_null_entity()
    {
        await this.session.SaveAsync(new TestEntity { Text = Guid.NewGuid().ToString() });
        await this.session.FlushAsync();

        await this.repository.DeleteAsync((TestEntity)null);

        Assert.Single(this.session.Query<TestEntity>().ToArray());
    }

    [Fact]
    public async Task Should_delete_an_entity()
    {
        var entity = new TestEntity { Text = Guid.NewGuid().ToString() };
        await this.session.SaveAsync(entity);
        await this.session.FlushAsync();

        await this.repository.DeleteAsync(entity);

        Assert.Empty(this.session.Query<TestEntity>().ToArray());
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

        foreach (var entity in entities.Where(r => r != null))
            await this.session.SaveAsync(entity);

        await this.repository.DeleteAsync(entities);

        Assert.Empty(this.session.Query<TestEntity>().ToArray());
    }

    [Fact]
    public async Task Should_ignore_empty_collection()
    {
        await this.repository.DeleteAsync(new TestEntity[] { null });

        Assert.Empty(this.session.Query<TestEntity>().ToArray());
    }
}