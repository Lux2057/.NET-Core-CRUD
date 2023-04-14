namespace Tests.DAL;

#region << Using >>

using CRUD.DAL;
using Tests.Models;

#endregion

public abstract class EfReadRepositoryTest : DbTest
{
    #region Properties

    protected readonly TestDbContext context;

    protected readonly IReadRepository<TestEntity> repository;

    #endregion

    #region Constructors

    protected EfReadRepositoryTest(TestDbContext context, IReadRepository<TestEntity> repository)
    {
        this.context = context;
        this.repository = repository;
    }

    #endregion
}