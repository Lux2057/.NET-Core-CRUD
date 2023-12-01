namespace NhTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using NHibernate;

#endregion

public class NhUnitOfWorkTest : DbTest
{
    #region Properties

    protected readonly ISession session;

    protected readonly IUnitOfWork UnitOfWork;

    protected IRepository Repository;

    #endregion

    #region Constructors

    public NhUnitOfWorkTest(IUnitOfWork unitOfWork,
                            IRepository repository,
                            ISession session)
    {
        this.UnitOfWork = unitOfWork;
        this.Repository = repository;
        this.session = session;
    }

    #endregion
}