namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using FluentValidation;
using JetBrains.Annotations;
using Samples.ToDo.Shared;

#endregion

public class CreateTagCommand : CommandBase
{
    #region Properties

    public string Name { get; }

    #endregion

    #region Constructors

    public CreateTagCommand(string name)
    {
        Name = name.Trim().ToLower();
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    public class Validator : AbstractValidator<CreateTagCommand>
    {
        #region Constructors

        public Validator(IDispatcher dispatcher)
        {
            RuleFor(r => r.Name).NotEmpty()
                                .MustAsync((name, _) => dispatcher.QueryAsync(new IsTagNameUniqueQuery(name))).WithMessage(ValidationMessagesConst.Name_is_not_unique);
        }

        #endregion
    }

    [UsedImplicitly]
    class Handler : CommandHandlerBase<CreateTagCommand>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(CreateTagCommand command, CancellationToken cancellationToken)
        {
            await Repository.CreateAsync(new TagEntity { Name = command.Name });
        }
    }

    #endregion
}