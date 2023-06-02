namespace EfTests.Core;

#region << Using >>

using CRUD.CQRS;

#endregion

public abstract class ReadDispatcherTest : DbTest
{
    #region Properties

    protected readonly TestDbContext context;

    protected readonly IReadDispatcher dispatcher;

    #endregion

    #region Constructors

    protected ReadDispatcherTest(IReadDispatcher dispatcher, TestDbContext context)
    {
        this.dispatcher = dispatcher;
        this.context = context;
    }

    #endregion
}