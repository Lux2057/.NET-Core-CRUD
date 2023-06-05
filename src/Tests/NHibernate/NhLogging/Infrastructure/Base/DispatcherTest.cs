namespace NhTests.Logging;

#region << Using >>

using CRUD.CQRS;

#endregion

public abstract class DispatcherTest : DbTest
{
    #region Properties

    protected IDispatcher Dispatcher { get; }

    #endregion

    #region Constructors

    protected DispatcherTest(IDispatcher dispatcher)
    {
        Dispatcher = dispatcher;
    }

    #endregion
}