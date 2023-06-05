namespace EfTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using EfTests.Shared;

#endregion

public abstract class EfRepositoryTest : DbTest
{
    #region Properties

    protected readonly TestDbContext context;

    protected readonly IRepository repository;

    #endregion

    #region Constructors

    protected EfRepositoryTest(TestDbContext context, IRepository repository)
    {
        this.context = context;
        this.repository = repository;
    }

    #endregion
}