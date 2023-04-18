namespace EfTests.CQRS;

#region << Using >>

using CRUD.CQRS;

#endregion

public class EfReadDispatcherTest : DbTest
{
    #region Properties

    protected TestDbContext context;

    protected IReadDispatcher dispatcher;

    #endregion

    #region Constructors

    public EfReadDispatcherTest(TestDbContext context, IReadDispatcher dispatcher)
    {
        this.context = context;
        this.dispatcher = dispatcher;
    }

    #endregion
}