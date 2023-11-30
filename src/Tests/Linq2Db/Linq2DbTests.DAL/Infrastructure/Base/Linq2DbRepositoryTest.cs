namespace Linq2DbTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using CRUD.DAL.Linq2Db;

#endregion

public abstract class Linq2DbRepositoryTest : DbTest
{
    #region Properties

    protected ILinq2DbRepository Linq2DbRepository { get; }

    protected IRepository Repository { get; }

    protected TestDataConnection Connection { get; }

    #endregion

    #region Constructors

    protected Linq2DbRepositoryTest(TestDataConnection connection, ILinq2DbRepository repository)
    {
        Connection = connection;
        Repository = repository;
        Linq2DbRepository = repository;
    }

    #endregion
}