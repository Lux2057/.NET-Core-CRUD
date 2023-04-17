namespace Tests.CQRS;

#region << Using >>

using CRUD.CQRS;
using Tests.Models;

#endregion

public class EfReadWriteDispatcherTest : DbTest
{
    #region Properties

    protected readonly TestDbContext context;

    protected readonly IReadWriteDispatcher dispatcher;

    #endregion

    #region Constructors

    public EfReadWriteDispatcherTest(TestDbContext context, IReadWriteDispatcher dispatcher)
    {
        this.context = context;
        this.dispatcher = dispatcher;
    }

    #endregion
}