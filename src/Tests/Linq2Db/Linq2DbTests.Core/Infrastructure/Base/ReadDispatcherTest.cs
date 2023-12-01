namespace Linq2DbTests.Core;

#region << Using >>

using CRUD.CQRS;

#endregion

public class ReadDispatcherTest : DbTest
{
    #region Properties

    protected TestDataConnection Connection { get; }

    protected IReadDispatcher Dispatcher { get; }

    #endregion

    #region Constructors

    public ReadDispatcherTest(TestDataConnection connection, IReadDispatcher dispatcher)
    {
        Connection = connection;
        Dispatcher = dispatcher;
    }

    #endregion
}