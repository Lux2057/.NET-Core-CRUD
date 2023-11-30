namespace Linq2DbTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using LinqToDB.Data;

#endregion

public abstract class Linq2DbRepositoryTest : DbTest
{
    #region Properties

    protected IRepository Repository { get; }

    protected DataConnection Connection { get; }

    #endregion

    #region Constructors

    protected Linq2DbRepositoryTest(DataConnection connection, IRepository repository)
    {
        Connection = connection;
        Repository = repository;
    }

    #endregion
}