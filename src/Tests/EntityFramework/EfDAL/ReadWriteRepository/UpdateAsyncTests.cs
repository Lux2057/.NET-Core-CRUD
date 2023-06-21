namespace EfTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using EfTests.Shared;
using Microsoft.EntityFrameworkCore;

#endregion

public class UpdateAsyncTests : EfRepositoryTest
{
    #region Constructors

    public UpdateAsyncTests(TestDbContext context, IRepository repository)
            : base(context, repository) { }

    #endregion

    [Fact]
    public async Task Should_update_an_entity()
    {
        var oldText = Guid.NewGuid().ToString();
        var newText = Guid.NewGuid().ToString();

        var entity = new TestEntity
                     {
                             Text = oldText
                     };

        var dbSet = this.context.Set<TestEntity>();
        dbSet.Add(entity);
        await this.context.SaveChangesAsync();

        entity.Text = newText;
        await this.repository.UpdateAsync(entity);

        var entityFromDb = await dbSet.SingleAsync();
        Assert.Equal(newText, entityFromDb.Text);
    }

    [Fact]
    public async Task Should_ignore_null()
    {
        await this.repository.UpdateAsync((TestEntity)null);

        var entitiesInDb = await this.context.Set<TestEntity>().ToArrayAsync();

        Assert.Empty(entitiesInDb);
    }

    [Fact]
    public async Task Should_throw_exception_for_nonexistent_entity()
    {
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>
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

        var entities = new[]
                       {
                               new TestEntity
                               {
                                       Text = oldText
                               },
                               new TestEntity
                               {
                                       Text = oldText
                               },
                               null
                       };

        var dbSet = this.context.Set<TestEntity>();
        await dbSet.AddRangeAsync(entities.Where(r => r != null));
        await this.context.SaveChangesAsync();

        entities[0].Text = newText;
        await this.repository.UpdateAsync(entities);

        var entitiesInDb = await dbSet.ToArrayAsync();
        Assert.Equal(newText, entitiesInDb[0].Text);
        Assert.Equal(oldText, entitiesInDb[1].Text);
    }

    [Fact]
    public async Task Should_ignore_empty_collection()
    {
        await this.repository.UpdateAsync(new TestEntity[] { null });

        Assert.Empty(await this.context.Set<TestEntity>().ToArrayAsync());
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

        await this.repository.CreateAsync(entities);

        foreach (var entity in entities)
            entity.Text = newText;

        entities = this.repository.Read<TestEntity>().ToArray();

        Assert.True(entities.All(x => x.Text == oldText));
    }
}