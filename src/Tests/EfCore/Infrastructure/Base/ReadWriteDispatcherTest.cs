namespace EfTests.Core;

#region << Using >>

using CRUD.CQRS;

#endregion

public abstract class ReadWriteDispatcherTest : DbTest
{
    #region Properties

    protected TestDbContext context;

    protected IReadWriteDispatcher dispatcher;

    #endregion

    #region Constructors

    protected ReadWriteDispatcherTest(IReadWriteDispatcher dispatcher, TestDbContext context)
    {
        this.dispatcher = dispatcher;
        this.context = context;
    }

    #endregion
}