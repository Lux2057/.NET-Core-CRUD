namespace EfTests.CQRS;

#region << Using >>

using CRUD.CQRS;

#endregion

public class ReadWriteDispatcherTest : DbTest
{
    #region Properties

    protected readonly TestDbContext context;

    protected readonly IReadWriteDispatcher dispatcher;

    #endregion

    #region Constructors

    public ReadWriteDispatcherTest(TestDbContext context, IReadWriteDispatcher dispatcher)
    {
        this.context = context;
        this.dispatcher = dispatcher;
    }

    #endregion
}