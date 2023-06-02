namespace NhTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using NHibernate;

#endregion

public abstract class NhReadRepositoryTest : DbTest
{
    #region Properties

    protected readonly IReadRepository repository;

    protected ISessionFactory sessionFactory;

    #endregion

    #region Constructors

    protected NhReadRepositoryTest(IReadRepository repository, ISessionFactory sessionFactory)
    {
        this.repository = repository;
        this.sessionFactory = sessionFactory;
    }

    #endregion
}