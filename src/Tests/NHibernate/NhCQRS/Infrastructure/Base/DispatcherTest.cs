namespace EfTests.CQRS;

#region << Using >>

using CRUD.CQRS;

#endregion

public class DispatcherTest : DbTest
{
    #region Properties

    protected readonly TestDbContext context;

    protected readonly IDispatcher dispatcher;

    #endregion

    #region Constructors

    public DispatcherTest(TestDbContext context, IDispatcher dispatcher)
    {
        this.context = context;
        this.dispatcher = dispatcher;
    }

    #endregion
}