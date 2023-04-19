namespace EfTests.Core;

#region << Using >>

using CRUD.CQRS;

#endregion

public abstract class ReadWriteDispatcherTest : DbTest
{
    #region Properties

    protected readonly IReadWriteDispatcher dispatcher;

    protected TestDbContext _context;

    #endregion

    #region Constructors

    protected ReadWriteDispatcherTest(TestDbContext context, IReadWriteDispatcher dispatcher)
    {
        this._context = context;
        this.dispatcher = dispatcher;
    }

    #endregion
}