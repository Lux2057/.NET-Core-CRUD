namespace Tests.DAL;

#region << Using >>

using CRUD.DAL;

#endregion

public abstract class ReadRepositoryTest : DbTest
{
    #region Properties

    protected readonly TestDbContext context;

    protected readonly IReadRepository<TestEntity> repository;

    #endregion

    #region Constructors

    protected ReadRepositoryTest(TestDbContext context, IReadRepository<TestEntity> repository)
    {
        this.context = context;
        this.repository = repository;
    }

    #endregion
}