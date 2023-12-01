namespace Linq2Db.CQRS.Specific;

#region << Using >>

using CRUD.CQRS;
using JetBrains.Annotations;

#endregion

internal class TestRollbackChangesCommand : CommandBase
{
    #region Properties

    public string TableName { get; }

    #endregion

    #region Constructors

    public TestRollbackChangesCommand(string tableName)
    {
        TableName = tableName;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Handler : CRUD.CQRS.Linq2Db.CommandHandlerBase<TestRollbackChangesCommand>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(TestRollbackChangesCommand command, CancellationToken cancellationToken)
        {
            var addOrUpdateTestEntityCommand = new AddOrUpdateTestEntityCommand(id: null,
                                                                                text: "TEST Text",
                                                                                tableName: command.TableName);

            await Dispatcher.PushAsync(addOrUpdateTestEntityCommand, cancellationToken);

            var dto = await Dispatcher.QueryAsync(new GetTestEntitiesByIdsQueryBase(ids: new[] { addOrUpdateTestEntityCommand.Result },
                                                                                    tableName: command.TableName),
                                                  cancellationToken: cancellationToken);

            throw new Exception("Test rollback");
        }
    }

    #endregion
}