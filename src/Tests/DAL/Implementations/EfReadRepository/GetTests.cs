namespace CRUD.Tests;

#region << Using >>

using CRUD.DAL;

#endregion

public class GetTests
{
    [Fact]
    public void Should_return_saved_entity()
    {
        var text = "Test1";
        var id = 1;

        using (var context = MockDbHelper.GetDbContextInstance())
        {
            var testEntity = new TestEntity
                             {
                                     Text = text
                             };

            context.Set<TestEntity>().Add(testEntity);
            context.SaveChanges();

            var repository = new EfReadRepository<TestEntity>(context);

            Assert.Single(repository.Get().ToArray());

            Assert.Equal(testEntity, repository.Get().Single());
            Assert.Equal(id, repository.Get().Single().Id);
            Assert.Equal(text, repository.Get().Single().Text);
        }
    }

    [Fact]
    public void Should_return_saved_entities_by_text_spec()
    {
        var text1 = "TEST";
        var text2 = "test";

        using (var context = MockDbHelper.GetDbContextInstance())
        {
            context.Set<TestEntity>().AddRange(new[]
                                               {
                                                       new TestEntity
                                                       {
                                                               Text = text1
                                                       },
                                                       new TestEntity
                                                       {
                                                               Text = text1
                                                       },
                                                       new TestEntity
                                                       {
                                                               Text = text2
                                                       }
                                               });

            context.SaveChanges();

            var repository = new EfReadRepository<TestEntity>(context);

            Assert.Equal(3, repository.Get().Count());
            Assert.Equal(2, repository.Get(new TestByTextSpecification(text1)).Count());
            Assert.Equal(1, repository.Get(new TestByTextSpecification(text2)).Count());
        }
    }
}