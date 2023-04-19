namespace EfTests.Core;

#region << Using >>

using CRUD.CQRS;

#endregion

public abstract class ReadWriteDispatcherTest : DbTest
{
    #region Properties

    protected readonly IReadWriteDispatcher dispatcher;

    #endregion

    #region Constructors

    protected ReadWriteDispatcherTest(IReadWriteDispatcher dispatcher)
    {
        this.dispatcher = dispatcher;
    }

    #endregion
}