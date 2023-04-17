namespace EfTests.CQRS;

#region << Using >>

using CRUD.CQRS;

#endregion

internal class TestRollbackChangesCommand : CommandBase
{
    #region Nested Classes

    class Handler : CommandHandlerBase<TestRollbackChangesCommand>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(TestRollbackChangesCommand command, CancellationToken cancellationToken)
        {
            var addOrUpdateTestEntityCommand = new AddOrUpdateTestEntityCommand { Text = "TEST Text" };
            await this.Dispatcher.PushAsync(addOrUpdateTestEntityCommand, cancellationToken);

            var dto = await this.Dispatcher.QueryAsync(new GetTestEntitiesByIdsQuery { Ids = new[] { addOrUpdateTestEntityCommand.Result } }, cancellationToken);

            throw new Exception("Test rollback");
        }
    }

    #endregion
}