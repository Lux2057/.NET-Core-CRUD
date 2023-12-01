namespace Linq2Db.CQRS.Specific;

#region << Using >>

using CRUD.CQRS;
using FluentValidation;
using JetBrains.Annotations;
using Linq2DbTests.Shared;

#endregion

internal class AddOrUpdateTestEntityCommand : CommandBase
{
    #region Properties

    public string Id { get; }

    public string Text { get; }

    public string TableName { get; }

    public new string Result { get; set; }

    #endregion

    #region Constructors

    public AddOrUpdateTestEntityCommand(string id, string text, string tableName)
    {
        Id = id;
        Text = text;
        TableName = tableName;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Validator : AbstractValidator<AddOrUpdateTestEntityCommand>
    {
        #region Constructors

        public Validator()
        {
            RuleFor(r => r.Text).NotEmpty();
        }

        #endregion
    }

    [UsedImplicitly]
    class Handler : CRUD.CQRS.Linq2Db.CommandHandlerBase<AddOrUpdateTestEntityCommand>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(AddOrUpdateTestEntityCommand command, CancellationToken cancellationToken)
        {
            var dto = (await Dispatcher.QueryAsync(new GetTestEntitiesByIdsQueryBase(ids: new[] { command.Id },
                                                                                     tableName: command.TableName), cancellationToken)).SingleOrDefault();

            TestEntity entity;
            if (dto == null)
            {
                entity = new TestEntity
                         {
                                 Text = command.Text
                         };

                await Repository.CreateAsync(entity: entity,
                                             tableName: command.TableName,
                                             cancellationToken: cancellationToken);
            }
            else
            {
                entity = Mapper.Map<TestEntity>(dto);
                entity.Text = command.Text;

                await Repository.UpdateAsync(entity: entity,
                                             tableName: command.TableName,
                                             cancellationToken: cancellationToken);
            }

            command.Result = entity.Id;
        }
    }

    #endregion
}