namespace NhTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using NHibernate;

#endregion

public abstract class NhReadRepositoryTest : DbTest
{
    #region Properties

    protected IReadRepository Repository { get; }

    protected ISessionFactory SessionFactory { get; }

    #endregion

    #region Constructors

    protected NhReadRepositoryTest(IReadRepository repository, ISessionFactory sessionFactory)
    {
        Repository = repository;
        SessionFactory = sessionFactory;
    }

    #endregion
}