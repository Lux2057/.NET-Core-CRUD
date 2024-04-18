namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using FluentValidation;
using JetBrains.Annotations;
using Samples.ToDo.Shared;

#endregion

public class DeleteProjectCommand : CommandBase, IDeleteEntityRequest
{
    #region Properties

    public int UserId { get; }

    public int Id { get; }

    public new bool Result { get; set; }

    #endregion

    #region Constructors

    public DeleteProjectCommand(int userId, int id)
    {
        UserId = userId;
        Id = id;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    public class Validator : AbstractValidator<DeleteProjectCommand>
    {
        #region Constructors

        public Validator(IDispatcher dispatcher)
        {
            RuleFor(r => r.Id).MustAsync((command, _, _) => dispatcher.QueryAsync(new DoesEntityBelongToUserQuery<ProjectEntity>(command.Id, command.UserId)))
                              .WithMessage(Localization.Project_id_is_invalid);

            RuleFor(r => r.UserId).NotEmpty().WithMessage(Localization.User_id_cant_be_empty)
                                  .MustAsync((id, _) => dispatcher.QueryAsync(new DoesEntityExistQuery<UserEntity>(id)))
                                  .WithMessage(Localization.User_id_is_invalid);
        }

        #endregion
    }

    [UsedImplicitly]
    class Handler : CommandHandlerBase<DeleteProjectCommand>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(DeleteProjectCommand command, CancellationToken cancellationToken)
        {
            var deleteCommand = new MarkEntitiesAsDeletedCommand<ProjectEntity>(new[] { command.Id });
            await Dispatcher.PushAsync(deleteCommand);

            command.Result = deleteCommand.Result;
        }
    }

    #endregion
}