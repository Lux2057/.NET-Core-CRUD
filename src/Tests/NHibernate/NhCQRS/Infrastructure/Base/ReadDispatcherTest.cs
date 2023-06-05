namespace NhTests.CQRS;

#region << Using >>

using CRUD.CQRS;
using NHibernate;

#endregion

public class ReadDispatcherTest : DbTest
{
    #region Properties

    protected ISessionFactory SessionFactory { get; }

    protected IReadDispatcher Dispatcher { get; }

    #endregion

    #region Constructors

    public ReadDispatcherTest(ISessionFactory sessionFactory, IReadDispatcher dispatcher)
    {
        SessionFactory = sessionFactory;
        Dispatcher = dispatcher;
    }

    #endregion
}