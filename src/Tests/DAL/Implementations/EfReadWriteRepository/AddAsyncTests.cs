namespace CRUD.Tests;

#region << Using >>

using CRUD.DAL;
using Microsoft.EntityFrameworkCore;

#endregion

public class AddAsyncTests
{
    [Fact]
    public void Should_add_single_entity()
    {
        var text = Guid.NewGuid().ToString();

        EfDbContextMocker.ExecuteWithDbContext(async context =>
                                          {
                                              var repository = new EfReadWriteRepository<TestEntity>(context);

                                              await repository.AddAsync(new TestEntity { Text = text });

                                              var entitiesInDb = await context.Set<TestEntity>().ToArrayAsync();

                                              Assert.Single(entitiesInDb);
                                              Assert.Equal(text, entitiesInDb[0].Text);
                                          });
    }

    [Fact]
    public void Should_ignore_null_entity()
    {
        EfDbContextMocker.ExecuteWithDbContext(async context =>
                                          {
                                              var repository = new EfReadWriteRepository<TestEntity>(context);

                                              await repository.AddAsync((TestEntity)null);

                                              var entitiesInDb = await context.Set<TestEntity>().ToArrayAsync();

                                              Assert.Empty(entitiesInDb);
                                          });
    }

    [Fact]
    public void Should_add_several_non_null_entities()
    {
        var text = Guid.NewGuid().ToString();

        var entities = new List<TestEntity>
                       {
                               new TestEntity
                               {
                                       Text = text
                               },
                               new TestEntity
                               {
                                       Text = text
                               },
                               (TestEntity)null
                       };

        EfDbContextMocker.ExecuteWithDbContext(async context =>
                                          {
                                              var repository = new EfReadWriteRepository<TestEntity>(context);

                                              await repository.AddAsync(entities);

                                              var entitiesInDb = await context.Set<TestEntity>().ToArrayAsync();

                                              Assert.Equal(2, entitiesInDb.Length);
                                              Assert.True(entitiesInDb.All(r => r.Text == text));
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

                                              await repository.AddAsync(entities);

                                              var entitiesInDb = await context.Set<TestEntity>().ToArrayAsync();

                                              Assert.Empty(entitiesInDb);
                                          });
    }
}