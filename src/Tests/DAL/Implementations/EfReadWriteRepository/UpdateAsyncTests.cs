namespace CRUD.Tests;

#region << Using >>

using CRUD.DAL;
using Microsoft.EntityFrameworkCore;

#endregion

public class UpdateAsyncTests
{
    [Fact]
    public void Should_update_an_entity()
    {
        var oldText = Guid.NewGuid().ToString();
        var newText = Guid.NewGuid().ToString();

        var entity = new TestEntity
                     {
                             Text = oldText
                     };

        MockDbHelper.ExecuteWithDbContext(async context =>
                                          {
                                              var dbSet = context.Set<TestEntity>();
                                              dbSet.Add(entity);
                                              await context.SaveChangesAsync();

                                              var repository = new EfReadWriteRepository<TestEntity>(context);
                                              entity.Text = newText;
                                              await repository.UpdateAsync(entity);

                                              var entityFromDb = await dbSet.SingleAsync();
                                              Assert.Equal(newText, entityFromDb.Text);
                                          });
    }

    [Fact]
    public void Should_ignore_null()
    {
        MockDbHelper.ExecuteWithDbContext(async context =>
                                          {
                                              var repository = new EfReadWriteRepository<TestEntity>(context);
                                              await repository.UpdateAsync((TestEntity)null);

                                              var entitiesInDb = await context.Set<TestEntity>().ToArrayAsync();

                                              Assert.Empty(entitiesInDb);
                                          });
    }

    [Fact]
    public void Should_throw_exception_for_nonexistent_entity()
    {
        var oldText = Guid.NewGuid().ToString();
        var newText = Guid.NewGuid().ToString();

        var entity = new TestEntity
                     {
                             Id = 1,
                             Text = oldText
                     };

        MockDbHelper.ExecuteWithDbContext(async context =>
                                          {
                                              var repository = new EfReadWriteRepository<TestEntity>(context);
                                              entity.Text = newText;

                                              await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await repository.UpdateAsync(entity));
                                          });
    }

    [Fact]
    public void Should_update_several_non_null_entities()
    {
        var oldText = Guid.NewGuid().ToString();
        var newText = Guid.NewGuid().ToString();

        var entities = new List<TestEntity>
                       {
                               new TestEntity
                               {
                                       Text = oldText
                               },
                               new TestEntity
                               {
                                       Text = oldText
                               },
                               (TestEntity)null
                       };

        MockDbHelper.ExecuteWithDbContext(async context =>
                                          {
                                              var dbSet = context.Set<TestEntity>();
                                              await dbSet.AddRangeAsync(entities.Where(r => r != null));
                                              await context.SaveChangesAsync();

                                              var repository = new EfReadWriteRepository<TestEntity>(context);
                                              entities[0].Text = newText;
                                              await repository.UpdateAsync(entities);

                                              var entitiesInDb = await dbSet.ToArrayAsync();
                                              Assert.Equal(newText, entitiesInDb[0].Text);
                                              Assert.Equal(oldText, entitiesInDb[1].Text);
                                          });
    }
}