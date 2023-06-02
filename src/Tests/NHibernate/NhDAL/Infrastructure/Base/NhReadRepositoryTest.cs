namespace NhTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using NHibernate;

#endregion

public abstract class NhReadRepositoryTest : DbTest
{
    #region Properties

    protected readonly IReadRepository repository;

    protected readonly ISession session;

    #endregion

    #region Constructors

    protected NhReadRepositoryTest(ISession session, IReadRepository repository)
    {
        this.session = session;
        this.repository = repository;
    }

    #endregion
}