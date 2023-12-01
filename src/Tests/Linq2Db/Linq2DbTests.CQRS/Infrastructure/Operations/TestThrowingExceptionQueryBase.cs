namespace Linq2Db.CQRS;

#region << Using >>

using CRUD.CQRS;

#endregion

#region << Using >>

#endregion

internal class TestThrowingExceptionQueryBase : QueryBase<bool>
{
    #region Nested Classes

    class Handler : QueryHandlerBase<TestThrowingExceptionQueryBase, bool>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override Task<bool> Execute(TestThrowingExceptionQueryBase request, CancellationToken cancellationToken)
        {
            throw new Exception("Test exception");
        }
    }

    #endregion
}