namespace Tests.CQRS;

#region << Using >>

using CRUD.CQRS;
using Tests.Models;

#endregion

public class ReadWriteDispatcherTest : DbTest
{
    #region Properties

    protected TestDbContext context;

    protected IReadWriteDispatcher dispatcher;

    #endregion

    #region Constructors

    public ReadWriteDispatcherTest(TestDbContext context, IReadWriteDispatcher dispatcher)
    {
        this.context = context;
        this.dispatcher = dispatcher;
    }

    #endregion
}