namespace EfTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using EfTests.Shared;
using Microsoft.EntityFrameworkCore;

#endregion

public class CreateAsyncTests : EfRepositoryTest
{
    #region Constructors

    public CreateAsyncTests(TestDbContext context, IRepository repository)
            : base(context, repository) { }

    #endregion

    [Fact]
    public async Task Should_create_single_entity()
    {
        var text = Guid.NewGuid().ToString();

        await this.repository.CreateAsync(new TestEntity { Text = text });

        var entitiesInDb = await this.context.Set<TestEntity>().ToArrayAsync();

        Assert.Single(entitiesInDb);
        Assert.Equal(text, entitiesInDb[0].Text);
    }

    [Fact]
    public async Task Should_ignore_null_entity()
    {
        await this.repository.CreateAsync((TestEntity)null);

        var entitiesInDb = await this.context.Set<TestEntity>().ToArrayAsync();

        Assert.Empty(entitiesInDb);
    }

    [Fact]
    public async Task Should_create_several_non_null_entities()
    {
        var text = Guid.NewGuid().ToString();

        await this.repository.CreateAsync(new TestEntity[]
                                       {
                                               new TestEntity
                                               {
                                                       Text = text
                                               },
                                               new TestEntity
                                               {
                                                       Text = text
                                               },
                                               null
                                       });

        var entitiesInDb = await this.context.Set<TestEntity>().ToArrayAsync();

        Assert.Equal(2, entitiesInDb.Length);
        Assert.True(entitiesInDb.All(r => r.Text == text));
    }

    [Fact]
    public async Task Should_ignore_empty_collection()
    {
        await this.repository.CreateAsync(new[] { (TestEntity)null });

        var entitiesInDb = await this.context.Set<TestEntity>().ToArrayAsync();

        Assert.Empty(entitiesInDb);
    }
}