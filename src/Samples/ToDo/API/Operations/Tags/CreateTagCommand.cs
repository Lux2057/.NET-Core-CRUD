namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using FluentValidation;
using JetBrains.Annotations;
using Samples.ToDo.API.Resources;
using Samples.ToDo.Shared;

#endregion

public class CreateTagCommand : CommandBase, ICreateTagRequest
{
    #region Properties

    public string Name { get; }

    public new bool Result { get; set; }

    #endregion

    #region Constructors

    public CreateTagCommand(string name)
    {
        Name = name?.Trim().ToLower() ?? string.Empty;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    public class Validator : AbstractValidator<CreateTagCommand>
    {
        #region Constructors

        public Validator(IDispatcher dispatcher)
        {
            RuleFor(r => r.Name).NotEmpty().WithMessage(Localization.Name_cant_be_empty)
                                .MustAsync((name, _) => dispatcher.QueryAsync(new IsTagNameUniqueQuery(name))).WithMessage(Localization.Name_is_not_unique);
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
            var tag = new TagEntity { Name = command.Name };
            await Repository.CreateAsync(tag);

            command.Result = true;
        }
    }

    #endregion
}