namespace NhTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using NHibernate;

#endregion

public abstract class NhRepositoryTest : DbTest
{
    #region Properties

    protected readonly IRepository repository;

    protected readonly ISession session;

    #endregion

    #region Constructors

    protected NhRepositoryTest(ISession session, IRepository repository)
    {
        this.session = session;
        this.repository = repository;
    }

    #endregion
}