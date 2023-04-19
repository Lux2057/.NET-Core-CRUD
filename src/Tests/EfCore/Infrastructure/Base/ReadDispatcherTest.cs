namespace EfTests.Core;

using CRUD.CQRS;

public abstract class ReadDispatcherTest : DbTest
{
    #region Properties

    protected readonly IReadDispatcher dispatcher;

    #endregion

    #region Constructors

    protected ReadDispatcherTest(IReadDispatcher dispatcher)
    {
        this.dispatcher = dispatcher;
    }

    #endregion
}