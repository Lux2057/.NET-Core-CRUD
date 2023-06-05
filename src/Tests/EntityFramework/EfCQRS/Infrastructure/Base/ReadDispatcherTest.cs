namespace EfTests.CQRS;

#region << Using >>

using CRUD.CQRS;
using EfTests.Shared;

#endregion

public class ReadDispatcherTest : DbTest
{
    #region Properties

    protected readonly TestDbContext context;

    protected readonly IReadDispatcher dispatcher;

    #endregion

    #region Constructors

    public ReadDispatcherTest(TestDbContext context, IReadDispatcher dispatcher)
    {
        this.context = context;
        this.dispatcher = dispatcher;
    }

    #endregion
}