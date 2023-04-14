namespace Tests.DAL;

#region << Using >>

using CRUD.DAL;

#endregion

public class GetTests : EfReadRepositoryTest
{
    #region Constructors

    public GetTests(TestDbContext context, IReadRepository<TestEntity> repository)
            : base(context, repository) { }

    #endregion

    [Fact]
    public void Should_return_saved_entity()
    {
        var text = Guid.NewGuid().ToString();

        var testEntity = new TestEntity
                         {
                                 Text = text
                         };

        this.context.Set<TestEntity>().Add(testEntity);
        this.context.SaveChanges();

        Assert.Single(this.repository.Get().ToArray());
        Assert.Equal(1, this.repository.Get().Single().Id);
        Assert.Equal(text, this.repository.Get().Single().Text);
    }

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
                                                });

        this.context.SaveChanges();

        Assert.Equal(3, this.repository.Get().Count());
        Assert.Equal(2, this.repository.Get(new TestByTextSpecification(text1)).Count());
        Assert.Equal(1, this.repository.Get(new TestByTextSpecification(text2)).Count());
    }
}