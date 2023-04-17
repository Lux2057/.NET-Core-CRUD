namespace Tests.CQRS;

#region << Using >>

using CRUD.CQRS;

#endregion

public class RollbackChangesCommand : CommandBase
{
    #region Nested Classes

    class Handler : CommandHandlerBase<RollbackChangesCommand>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(RollbackChangesCommand command, CancellationToken cancellationToken)
        {
            var addOrUpdateTestEntityCommand = new AddOrUpdateTestEntityCommand { Text = "TEST Text" };
            await this.Dispatcher.PushAsync(addOrUpdateTestEntityCommand, cancellationToken);

            var dto = this.Dispatcher.QueryAsync(new GetTestEntitiesByIdsQuery { Ids = new[] { addOrUpdateTestEntityCommand.Result } }, cancellationToken);

            throw new Exception("Test rollback");
        }
    }

    #endregion
}