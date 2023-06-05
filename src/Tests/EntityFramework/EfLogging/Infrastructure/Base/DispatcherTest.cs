namespace EfTests.Logging;

#region << Using >>

using CRUD.CQRS;

#endregion

public abstract class DispatcherTest : DbTest
{
    #region Properties

    protected readonly TestDbContext context;

    protected readonly IDispatcher dispatcher;

    #endregion

    #region Constructors

    protected DispatcherTest(TestDbContext context, IDispatcher dispatcher)
    {
        this.context = context;
        this.dispatcher = dispatcher;
    }

    #endregion
}