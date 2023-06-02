namespace EfTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;

#endregion

public class EfUnitOfWorkTest : DbTest
{
    #region Properties

    protected readonly TestDbContext context;

    protected readonly IScopedUnitOfWork ScopedUnitOfWork;

    #endregion

    #region Constructors

    public EfUnitOfWorkTest(IScopedUnitOfWork scopedUnitOfWork, TestDbContext context)
    {
        this.ScopedUnitOfWork = scopedUnitOfWork;
        this.context = context;
    }

    #endregion
}