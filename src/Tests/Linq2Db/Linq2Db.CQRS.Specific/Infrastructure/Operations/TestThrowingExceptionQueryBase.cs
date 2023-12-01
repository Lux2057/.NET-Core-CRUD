namespace Linq2Db.CQRS.Specific;

#region << Using >>

using CRUD.CQRS;

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