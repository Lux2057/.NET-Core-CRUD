namespace NhTests.Core;

#region << Using >>

using CRUD.CQRS;
using NHibernate;

#endregion

public abstract class ReadDispatcherTest : DbTest
{
    #region Properties

    protected ISessionFactory SessionFactory { get; }

    protected IReadDispatcher Dispatcher { get; }

    #endregion

    #region Constructors

    protected ReadDispatcherTest(IReadDispatcher dispatcher, ISessionFactory sessionFactory)
    {
        Dispatcher = dispatcher;
        SessionFactory = sessionFactory;
    }

    #endregion
}