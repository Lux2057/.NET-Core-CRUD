namespace NhTests.CQRS;

#region << Using >>

using CRUD.CQRS;
using NhTests.Shared;

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
            await Repository.AddAsync(new TestEntity
                                      {
                                              Text = command.Text
                                      }, cancellationToken);
        }
    }

    #endregion
}