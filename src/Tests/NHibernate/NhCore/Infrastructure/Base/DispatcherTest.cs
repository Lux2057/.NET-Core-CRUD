namespace NhTests.Core;

#region << Using >>

using CRUD.CQRS;
using NHibernate;

#endregion

public abstract class DispatcherTest : DbTest
{
    #region Properties

    protected IDispatcher Dispatcher { get; }

    protected ISessionFactory SessionFactory { get; }

    #endregion

    #region Constructors

    protected DispatcherTest(ISessionFactory sessionFactory, IDispatcher dispatcher)
    {
        SessionFactory = sessionFactory;
        Dispatcher = dispatcher;
    }

    #endregion
}