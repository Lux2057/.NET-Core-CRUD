namespace Linq2DbTests.Logging;

#region << Using >>

using CRUD.CQRS;

#endregion

public class DispatcherTest : DbTest
{
    #region Properties

    protected IDispatcher Dispatcher { get; }

    protected TestDataConnection Connection { get; }

    #endregion

    #region Constructors

    public DispatcherTest(IDispatcher dispatcher, TestDataConnection connection)
    {
        Dispatcher = dispatcher;
        Connection = connection;
    }

    #endregion
}