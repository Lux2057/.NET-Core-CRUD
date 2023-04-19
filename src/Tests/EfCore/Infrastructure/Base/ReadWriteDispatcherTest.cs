namespace EfTests.Core;

#region << Using >>

using CRUD.CQRS;

#endregion

public abstract class ReadWriteDispatcherTest : DbTest
{
    #region Properties

    protected readonly IReadWriteDispatcher dispatcher;

    protected readonly TestDbContext context;

    #endregion

    #region Constructors

    protected ReadWriteDispatcherTest(TestDbContext context, IReadWriteDispatcher dispatcher)
    {
        this.context = context;
        this.dispatcher = dispatcher;
    }

    #endregion
}