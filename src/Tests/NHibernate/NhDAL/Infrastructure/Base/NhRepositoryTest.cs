namespace NhTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using NHibernate;

#endregion

public abstract class NhRepositoryTest : DbTest
{
    #region Properties

    protected readonly IRepository repository;

    protected readonly ISessionFactory sessionFactory;

    #endregion

    #region Constructors

    protected NhRepositoryTest(ISessionFactory sessionFactory, IRepository repository)
    {
        this.sessionFactory = sessionFactory;
        this.repository = repository;
    }

    #endregion
}