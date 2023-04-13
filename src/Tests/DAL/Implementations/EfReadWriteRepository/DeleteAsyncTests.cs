namespace CRUD.Tests;

#region << Using >>

using CRUD.DAL;
using Microsoft.EntityFrameworkCore;

#endregion

public class DeleteAsyncTests
{
    [Fact]
    public void Should_ignore_null_entity()
    {
        EfDbContextMocker.ExecuteWithDbContext(async context =>
                                          {
                                              var dbSet = context.Set<TestEntity>();
                                              await dbSet.AddAsync(new TestEntity { Text = Guid.NewGuid().ToString() });
                                              await context.SaveChangesAsync();

                                              var repository = new EfReadWriteRepository<TestEntity>(context);
                                              await repository.DeleteAsync((TestEntity)null);

                                              Assert.Single(await dbSet.ToArrayAsync());
                                          });
    }

    [Fact]
    public void Should_delete_an_entity()
    {
        EfDbContextMocker.ExecuteWithDbContext(async context =>
                                          {
                                              var dbSet = context.Set<TestEntity>();
                                              var entity = new TestEntity { Text = Guid.NewGuid().ToString() };
                                              await dbSet.AddAsync(entity);
                                              await context.SaveChangesAsync();

                                              var repository = new EfReadWriteRepository<TestEntity>(context);
                                              await repository.DeleteAsync(entity);

                                              Assert.Empty(await dbSet.ToArrayAsync());
                                          });
    }

    [Fact]
    public void Should_throw_exception_for_an_unattached_entity()
    {
        EfDbContextMocker.ExecuteWithDbContext(async context =>
                                          {
                                              var repository = new EfReadWriteRepository<TestEntity>(context);

                                              await Assert.ThrowsAsync<DbUpdateConcurrencyException>
                                                      (async () =>
                                                               await repository.DeleteAsync(new TestEntity
                                                                                            {
                                                                                                    Id = 1,
                                                                                                    Text = Guid.NewGuid().ToString()
                                                                                            }));
                                          });
    }

    [Fact]
    public void Should_delete_several_non_null_entities()
    {
        var entities = new List<TestEntity>
                       {
                               new TestEntity
                               {
                                       Text = Guid.NewGuid().ToString()
                               },
                               new TestEntity
                               {
                                       Text = Guid.NewGuid().ToString()
                               },
                               (TestEntity)null
                       };

        EfDbContextMocker.ExecuteWithDbContext(async context =>
                                          {
                                              var dbSet = context.Set<TestEntity>();
                                              await dbSet.AddRangeAsync(entities.Where(r => r != null));
                                              await context.SaveChangesAsync();

                                              var repository = new EfReadWriteRepository<TestEntity>(context);
                                              await repository.DeleteAsync(entities);

                                              Assert.Empty(await dbSet.ToArrayAsync());
                                          });
    }

    [Fact]
    public void Should_ignore_empty_collection()
    {
        var entities = new List<TestEntity>
                       {
                               (TestEntity)null
                       };

        EfDbContextMocker.ExecuteWithDbContext(async context =>
                                          {
                                              var repository = new EfReadWriteRepository<TestEntity>(context);

                                              await repository.DeleteAsync(entities);

                                              Assert.Empty(await context.Set<TestEntity>().ToArrayAsync());
                                          });
    }
}