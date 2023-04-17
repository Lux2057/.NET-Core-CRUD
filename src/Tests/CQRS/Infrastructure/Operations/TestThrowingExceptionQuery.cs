namespace Tests.CQRS;

#region << Using >>

using CRUD.CQRS;

#endregion

internal class TestThrowingExceptionQuery : IQuery<bool>
{
    #region Nested Classes

    class Handler : QueryHandlerBase<TestThrowingExceptionQuery, bool>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override Task<bool> Execute(TestThrowingExceptionQuery request, CancellationToken cancellationToken)
        {
            throw new Exception("Test exception");
        }
    }

    #endregion
}