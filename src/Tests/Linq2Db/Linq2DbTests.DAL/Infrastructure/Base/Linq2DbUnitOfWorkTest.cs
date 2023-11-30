namespace Linq2DbTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using LinqToDB.Data;

#endregion

public class Linq2DbUnitOfWorkTest : DbTest
{
    #region Properties

    protected readonly IUnitOfWork UnitOfWork;

    protected readonly DataConnection Connection;

    #endregion

    #region Constructors

    public Linq2DbUnitOfWorkTest(IUnitOfWork unitOfWork, DataConnection connection)
    {
        this.UnitOfWork = unitOfWork;
        this.Connection = connection;
    }

    #endregion
}