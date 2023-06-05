namespace EfTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using EfTests.Shared;

#endregion

public abstract class EfReadRepositoryTest : DbTest
{
    #region Properties

    protected readonly TestDbContext context;

    protected readonly IReadRepository repository;

    #endregion

    #region Constructors

    protected EfReadRepositoryTest(TestDbContext context, IReadRepository repository)
    {
        this.context = context;
        this.repository = repository;
    }

    #endregion
}