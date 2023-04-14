namespace Tests.Infrastructure;

#region << Using >>

using CRUD.DAL;

#endregion

public class EfUnitOfWorkTest : DbTest
{
    #region Properties

    protected readonly TestDbContext context;

    protected readonly IUnitOfWork unitOfWork;

    #endregion

    #region Constructors

    public EfUnitOfWorkTest(IUnitOfWork unitOfWork, TestDbContext context)
    {
        this.unitOfWork = unitOfWork;
        this.context = context;
    }

    #endregion
}