namespace EfTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using EfTests.Shared;

#endregion

public class EfUnitOfWorkTest : DbTest
{
    #region Properties

    protected readonly TestDbContext context;

    protected readonly IRepository Repository;

    protected readonly IUnitOfWork UnitOfWork;

    #endregion

    #region Constructors

    public EfUnitOfWorkTest(IUnitOfWork unitOfWork, 
                            IRepository repository, 
                            TestDbContext context)
    {
        this.UnitOfWork = unitOfWork;
        this.context = context;
        this.Repository = repository;
    }

    #endregion
}