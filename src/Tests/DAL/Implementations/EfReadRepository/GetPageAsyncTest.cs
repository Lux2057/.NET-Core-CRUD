namespace CRUD.Tests;

#region << Using >>

using CRUD.DAL;
using Microsoft.EntityFrameworkCore;

#endregion

public class GetPageAsyncTest
{
    [Fact]
    public void Should_return_saved_entities_by_text_spec()
    {
        var text1 = Guid.NewGuid().ToString();
        var text2 = Guid.NewGuid().ToString();

        MockDbHelper.ExecuteWithDbContext(async context =>
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

                                              await context.SaveChangesAsync();

                                              var repository = new EfReadRepository<TestEntity>(context);

                                              var testPage1 = await (await repository.GetPageAsync(page: 1, pageSize: 3)).ToArrayAsync();
                                              Assert.Equal(3, testPage1.Count());
                                              Assert.Equal(text2, testPage1[2].Text);

                                              var testPage2 = await (await repository.GetPageAsync(specification: new TestByTextSpecification(text1), page: 2, pageSize: 1)).ToArrayAsync();
                                              Assert.Single(testPage2);
                                              Assert.Equal(2, testPage2.Single().Id);
                                          });
    }
}