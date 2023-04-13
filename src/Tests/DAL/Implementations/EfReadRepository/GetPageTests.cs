namespace CRUD.Tests;

#region << Using >>

using CRUD.DAL;

#endregion

public class GetPageTests
{
    [Fact]
    public void Should_return_saved_entities_by_text_spec()
    {
        var text1 = Guid.NewGuid().ToString();
        var text2 = Guid.NewGuid().ToString();

        MockDbHelper.ExecuteWithDbContext(context =>
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
                                                                                         },
                                                                                         new TestEntity
                                                                                         {
                                                                                                 Text = text2
                                                                                         },
                                                                                         new TestEntity
                                                                                         {
                                                                                                 Text = text2
                                                                                         }
                                                                                 });

                                              context.SaveChanges();

                                              var repository = new EfReadRepository<TestEntity>(context);

                                              var testPage1 = repository.GetPage(page: 1, pageSize: 3).ToArray();
                                              Assert.Equal(3, testPage1.Count());
                                              Assert.Equal(text2, testPage1[2].Text);

                                              var testPage2 = repository.GetPage(specification: new TestByTextSpecification(text1), page: 2, pageSize: 1).ToArray();
                                              Assert.Single(testPage2);
                                              Assert.Equal(2, testPage2.Single().Id);
                                          });
    }
}