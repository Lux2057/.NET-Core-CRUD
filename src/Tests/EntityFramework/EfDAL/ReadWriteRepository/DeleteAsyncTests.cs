namespace EfTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using Microsoft.EntityFrameworkCore;

#endregion

public class DeleteAsyncTests : EfRepositoryTest
{
    #region Constructors

    public DeleteAsyncTests(TestDbContext context, IRepository repository)
            : base(context, repository) { }

    #endregion

    [Fact]
    public async Task Should_ignore_null_entity()
    {
        var dbSet = this.context.Set<TestEntity>();
        await dbSet.AddAsync(new TestEntity { Text = Guid.NewGuid().ToString() });
        await this.context.SaveChangesAsync();

        await this.repository.DeleteAsync((TestEntity)null);

        Assert.Single(await dbSet.ToArrayAsync());
    }

    [Fact]
    public async Task Should_delete_an_entity()
    {
        var dbSet = this.context.Set<TestEntity>();
        var entity = new TestEntity { Text = Guid.NewGuid().ToString() };
        await dbSet.AddAsync(entity);
        await this.context.SaveChangesAsync();

        await this.repository.DeleteAsync(entity);

        Assert.Empty(await dbSet.ToArrayAsync());
    }

    [Fact]
    public async Task Should_throw_exception_for_an_unattached_entity()
    {
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>
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
                               new TestEntity
                               {
                                       Text = Guid.NewGuid().ToString()
                               },
                               new TestEntity
                               {
                                       Text = Guid.NewGuid().ToString()
                               },
                               null
                       };

        var dbSet = this.context.Set<TestEntity>();
        await dbSet.AddRangeAsync(entities.Where(r => r != null));
        await this.context.SaveChangesAsync();

        await this.repository.DeleteAsync(entities);

        Assert.Empty(await dbSet.ToArrayAsync());
    }

    [Fact]
    public async Task Should_ignore_empty_collection()
    {
        await this.repository.DeleteAsync(new TestEntity[] { null });

        Assert.Empty(await this.context.Set<TestEntity>().ToArrayAsync());
    }
}