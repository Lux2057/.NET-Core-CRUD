namespace EfTests.CQRS;

#region << Using >>

using CRUD.CQRS;
using JetBrains.Annotations;

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
            await Repository<TestEntity>().AddAsync(new[]
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