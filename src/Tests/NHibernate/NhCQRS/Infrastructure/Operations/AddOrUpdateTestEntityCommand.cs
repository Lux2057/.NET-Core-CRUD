namespace NhTests.CQRS;

#region << Using >>

using CRUD.CQRS;
using FluentValidation;
using JetBrains.Annotations;
using NhTests.Shared;

#endregion

internal class AddOrUpdateTestEntityCommand : CommandBase
{
    #region Properties

    public int? Id { get; init; }

    public string Text { get; init; }

    public new int Result { get; set; }

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
    class Handler : CommandHandlerBase<AddOrUpdateTestEntityCommand>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(AddOrUpdateTestEntityCommand command, CancellationToken cancellationToken)
        {
            var dto = (await Dispatcher.QueryAsync(new GetTestEntitiesByIdsQueryBase(new[] { command.Id.GetValueOrDefault(0) }),
                                                   cancellationToken)).SingleOrDefault();

            TestEntity entity;
            if (dto == null)
            {
                entity = new TestEntity
                         {
                                 Text = command.Text
                         };

                await Repository.CreateAsync(entity, cancellationToken);
            }
            else
            {
                entity = Mapper.Map<TestEntity>(dto);
                entity.Text = command.Text;

                await Repository.UpdateAsync(entity, cancellationToken);
            }

            command.Result = entity.Id;
        }
    }

    #endregion
}