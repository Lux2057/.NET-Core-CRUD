namespace NhTests.DAL;

#region << Using >>

using CRUD.DAL.Abstractions;
using NHibernate;

#endregion

public abstract class NhRepositoryTest : DbTest
{
    #region Properties

    protected IRepository Repository { get; }

    protected ISessionFactory SessionFactory { get; }

    #endregion

    #region Constructors

    protected NhRepositoryTest(ISessionFactory sessionFactory, IRepository repository)
    {
        SessionFactory = sessionFactory;
        Repository = repository;
    }

    #endregion
}