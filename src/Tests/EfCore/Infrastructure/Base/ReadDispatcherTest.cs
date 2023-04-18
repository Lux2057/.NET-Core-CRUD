namespace EfTests.Core;

using CRUD.CQRS;

public abstract class ReadDispatcherTest : DbTest
{
    #region Properties

    protected TestDbContext context;

    protected IReadDispatcher dispatcher;

    #endregion

    #region Constructors

    protected ReadDispatcherTest(TestDbContext context, IReadDispatcher dispatcher)
    {
        this.context = context;
        this.dispatcher = dispatcher;
    }

    #endregion
}