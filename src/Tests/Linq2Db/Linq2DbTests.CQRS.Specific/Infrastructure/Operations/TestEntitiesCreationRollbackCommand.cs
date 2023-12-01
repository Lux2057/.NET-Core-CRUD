namespace Linq2Db.CQRS.Specific;

#region << Using >>

using CRUD.CQRS;
using JetBrains.Annotations;
using Linq2DbTests.Shared;

#endregion

public class TestEntitiesCreationRollbackCommand : CommandBase
{
    #region Properties

    public string TableName { get; }

    #endregion

    #region Constructors

    public TestEntitiesCreationRollbackCommand(string tableName)
    {
        TableName = tableName;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Handler : CRUD.CQRS.Linq2Db.CommandHandlerBase<TestEntitiesCreationRollbackCommand>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(TestEntitiesCreationRollbackCommand command, CancellationToken cancellationToken)
        {
            var text = Guid.NewGuid().ToString();
            await Repository.CreateAsync(entities: new[]
                                                   {
                                                           new TestEntity { Text = text },
                                                           new TestEntity { Text = text },
                                                           new TestEntity { Text = text },
                                                           new TestEntity { Text = text }
                                                   },
                                         tableName: command.TableName,
                                         cancellationToken: cancellationToken);

            throw new Exception("Test rollback");
        }
    }

    #endregion
}