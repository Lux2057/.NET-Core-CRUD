namespace EfTests.Core;

#region << Using >>

using CRUD.CQRS;
using EfTests.Shared;

#endregion

public abstract class DispatcherTest : DbTest
{
    #region Properties

    protected readonly IDispatcher dispatcher;

    protected readonly TestDbContext context;

    #endregion

    #region Constructors

    protected DispatcherTest(TestDbContext context, IDispatcher dispatcher)
    {
        this.context = context;
        this.dispatcher = dispatcher;
    }

    #endregion
}