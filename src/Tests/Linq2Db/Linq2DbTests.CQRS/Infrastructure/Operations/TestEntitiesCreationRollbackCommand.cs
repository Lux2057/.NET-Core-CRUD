namespace Linq2Db.CQRS;

#region << Using >>

using CRUD.CQRS;
using JetBrains.Annotations;
using Linq2DbTests.Shared;

#endregion

public class TestEntitiesCreationRollbackCommand : CommandBase
{
    #region Nested Classes

    [UsedImplicitly]
    class Handler : CommandHandlerBase<TestEntitiesCreationRollbackCommand>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(TestEntitiesCreationRollbackCommand command, CancellationToken cancellationToken)
        {
            var text = Guid.NewGuid().ToString();
            await Repository.CreateAsync(new[]
                                         {
                                                 new TestEntity { Text = text },
                                                 new TestEntity { Text = text },
                                                 new TestEntity { Text = text },
                                                 new TestEntity { Text = text }
                                         }, cancellationToken);

            throw new Exception("Test rollback");
        }
    }

    #endregion
}