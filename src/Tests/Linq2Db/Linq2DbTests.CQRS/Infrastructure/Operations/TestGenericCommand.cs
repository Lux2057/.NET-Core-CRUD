namespace Linq2Db.CQRS;

#region << Using >>

using CRUD.CQRS;
using Linq2DbTests.Shared;

#endregion

#region << Using >>

#endregion

public class TestGenericCommand<T> : CommandBase where T : TestEntity, new()
{
    #region Properties

    public string Text { get; set; }

    #endregion

    #region Nested Classes

    public class Handler : CommandHandlerBase<TestGenericCommand<T>>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(TestGenericCommand<T> command, CancellationToken cancellationToken)
        {
            await Repository.CreateAsync(new TestEntity
                                         {
                                                 Text = command.Text
                                         }, cancellationToken);
        }
    }

    #endregion
}