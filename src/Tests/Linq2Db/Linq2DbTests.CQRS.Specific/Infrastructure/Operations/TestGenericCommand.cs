namespace Linq2Db.CQRS.Specific;

#region << Using >>

using CRUD.CQRS;
using Linq2DbTests.Shared;

#endregion

#region << Using >>

#endregion

public class TestGenericCommand<T> : CommandBase where T : TestEntity, new()
{
    #region Properties

    public string Text { get; }

    public string TableName { get; }

    #endregion

    #region Constructors

    public TestGenericCommand(string text, string tableName)
    {
        TableName = tableName;
        Text = text;
    }

    #endregion

    #region Nested Classes

    public class Handler : CRUD.CQRS.Linq2Db.CommandHandlerBase<TestGenericCommand<T>>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(TestGenericCommand<T> command, CancellationToken cancellationToken)
        {
            await Repository.CreateAsync(entity: new TestEntity
                                                 {
                                                         Text = command.Text
                                                 },
                                         tableName: command.TableName,
                                         cancellationToken: cancellationToken);
        }
    }

    #endregion
}