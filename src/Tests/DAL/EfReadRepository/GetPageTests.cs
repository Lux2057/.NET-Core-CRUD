namespace Tests.DAL;

#region << Using >>

using CRUD.DAL;
using Tests.Infrastructure;

#endregion

public class GetPageTests : EfReadRepositoryTest
{
    #region Constructors

    public GetPageTests(TestDbContext context, IReadRepository<TestEntity> repository)
            : base(context, repository) { }

    #endregion

    [Fact]
    public void Should_return_saved_entities_by_text_spec()
    {
        var text1 = Guid.NewGuid().ToString();
        var text2 = Guid.NewGuid().ToString();

        this.context.Set<TestEntity>().AddRange(new TestEntity
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
                                                });

        this.context.SaveChanges();

        var testPage1 = this.repository.GetPage(page: 1, pageSize: 3).ToArray();
        Assert.Equal(3, testPage1.Count());
        Assert.Equal(text2, testPage1[2].Text);

        var testPage2 = this.repository.GetPage(specification: new TestByTextSpecification(text1), page: 2, pageSize: 1).ToArray();
        Assert.Single(testPage2);
        Assert.Equal(2, testPage2.Single().Id);
    }
}