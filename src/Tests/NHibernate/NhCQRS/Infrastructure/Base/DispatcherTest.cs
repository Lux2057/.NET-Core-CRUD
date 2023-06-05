namespace NhTests.CQRS;

#region << Using >>

using CRUD.CQRS;
using NHibernate;

#endregion

public class DispatcherTest : DbTest
{
    #region Properties

    protected ISessionFactory SessionFactory { get; }

    protected IDispatcher Dispatcher { get; }

    #endregion

    #region Constructors

    public DispatcherTest(ISessionFactory sessionFactory, IDispatcher dispatcher)
    {
        SessionFactory = sessionFactory;
        Dispatcher = dispatcher;
    }

    #endregion
}