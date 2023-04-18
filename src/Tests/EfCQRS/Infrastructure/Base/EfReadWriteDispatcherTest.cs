namespace EfTests.CQRS;

#region << Using >>

using CRUD.CQRS;

#endregion

public class EfReadWriteDispatcherTest : DbTest
{
    #region Properties

    protected TestDbContext context;

    protected IReadWriteDispatcher dispatcher;

    #endregion

    #region Constructors

    public EfReadWriteDispatcherTest(TestDbContext context, IReadWriteDispatcher dispatcher)
    {
        this.context = context;
        this.dispatcher = dispatcher;
    }

    #endregion
}