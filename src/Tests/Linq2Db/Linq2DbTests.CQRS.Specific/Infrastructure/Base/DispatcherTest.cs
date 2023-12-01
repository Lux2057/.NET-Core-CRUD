namespace Linq2Db.CQRS.Specific;

#region << Using >>

using CRUD.CQRS;

#endregion

public class DispatcherTest : DbTest
{
    #region Properties

    protected TestDataConnection Connection { get; }

    protected IDispatcher Dispatcher { get; }

    #endregion

    #region Constructors

    public DispatcherTest(TestDataConnection connection, IDispatcher dispatcher)
    {
        Connection = connection;
        Dispatcher = dispatcher;
    }

    #endregion
}