namespace Linq2Db.CQRS;

#region << Using >>

using CRUD.CQRS;
using JetBrains.Annotations;

#endregion

internal class TestRollbackChangesCommand : CommandBase
{
    #region Nested Classes

    [UsedImplicitly]
    class Handler : CommandHandlerBase<TestRollbackChangesCommand>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(TestRollbackChangesCommand command, CancellationToken cancellationToken)
        {
            var addOrUpdateTestEntityCommand = new AddOrUpdateTestEntityCommand { Text = "TEST Text" };
            await Dispatcher.PushAsync(addOrUpdateTestEntityCommand, cancellationToken);

            var dto = await Dispatcher.QueryAsync(new GetTestEntitiesByIdsQueryBase(new[] { addOrUpdateTestEntityCommand.Result }), cancellationToken);

            throw new Exception("Test rollback");
        }
    }

    #endregion
}