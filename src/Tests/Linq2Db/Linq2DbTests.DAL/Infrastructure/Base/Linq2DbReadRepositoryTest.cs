namespace Linq2DbTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using CRUD.DAL.Linq2Db;
using LinqToDB.Data;

#endregion

public abstract class Linq2DbReadRepositoryTest : DbTest
{
    #region Properties

    protected ILinq2DbRepository Linq2DbRepository { get; }

    protected IRepository Repository { get; }

    protected DataConnection Connection { get; }

    #endregion

    #region Constructors

    protected Linq2DbReadRepositoryTest(ILinq2DbRepository repository, DataConnection connection)
    {
        Repository = repository;
        Linq2DbRepository = repository;
        Connection = connection;
    }

    #endregion
}