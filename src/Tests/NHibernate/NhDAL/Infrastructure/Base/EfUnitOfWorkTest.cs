namespace NhTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using NHibernate;

#endregion

public class EfUnitOfWorkTest : DbTest
{
    #region Properties

    protected readonly IUnitOfWork UnitOfWork;

    protected readonly ISession session;

    #endregion

    #region Constructors

    public EfUnitOfWorkTest(IUnitOfWork unitOfWork, ISession session)
    {
        this.UnitOfWork = unitOfWork;
        this.session = session;
    }

    #endregion
}