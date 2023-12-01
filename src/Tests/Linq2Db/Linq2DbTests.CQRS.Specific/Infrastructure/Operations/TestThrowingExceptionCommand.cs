namespace Linq2Db.CQRS.Specific;

#region << Using >>

using CRUD.CQRS;
using JetBrains.Annotations;

#endregion

internal class TestThrowingExceptionCommand : CommandBase
{
    #region Nested Classes

    [UsedImplicitly]
    class Handler : CommandHandlerBase<TestThrowingExceptionCommand>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override Task Execute(TestThrowingExceptionCommand command, CancellationToken cancellationToken)
        {
            throw new Exception("Test exception");
        }
    }

    #endregion
}