namespace Tests.CQRS;

#region << Using >>

using CRUD.CQRS;

#endregion

internal class TestRollbackCommand : CommandBase
{
    #region Nested Classes

    class Handler : CommandHandlerBase<TestRollbackCommand>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(TestRollbackCommand command, CancellationToken cancellationToken)
        {
            await this.Dispatcher.PushAsync(new AddOrUpdateTestEntityCommand { Text = Guid.NewGuid().ToString() }, cancellationToken);

            await this.Dispatcher.PushAsync(new TestThrowingExceptionCommand(), cancellationToken);
        }
    }

    #endregion
}