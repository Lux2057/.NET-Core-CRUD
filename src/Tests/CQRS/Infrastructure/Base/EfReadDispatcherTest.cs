namespace Tests.CQRS;

#region << Using >>

using CRUD.CQRS;
using Tests.Models;

#endregion

public class EfReadDispatcherTest : DbTest
{
    #region Properties

    protected readonly TestDbContext context;

    protected readonly IReadDispatcher dispatcher;

    #endregion

    #region Constructors

    public EfReadDispatcherTest(TestDbContext context, IReadDispatcher dispatcher)
    {
        this.context = context;
        this.dispatcher = dispatcher;
    }

    #endregion
}