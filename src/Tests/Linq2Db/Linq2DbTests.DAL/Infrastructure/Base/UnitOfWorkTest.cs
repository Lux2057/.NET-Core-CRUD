namespace Linq2DbTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using CRUD.DAL.Linq2Db;

#endregion

public class UnitOfWorkTest : DbTest
{
    #region Properties

    protected ILinq2DbRepository Linq2DbRepository { get; }

    protected IRepository Repository { get; }

    protected readonly TestDataConnection Connection;

    protected readonly IUnitOfWork UnitOfWork;

    #endregion

    #region Constructors

    public UnitOfWorkTest(IUnitOfWork unitOfWork,
                          ILinq2DbRepository repository,
                          TestDataConnection connection)
    {
        this.UnitOfWork = unitOfWork;
        Linq2DbRepository = repository;
        Repository = repository;
        this.Connection = connection;
    }

    #endregion
}