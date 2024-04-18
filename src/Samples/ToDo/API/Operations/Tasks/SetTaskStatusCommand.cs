namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Samples.ToDo.Shared;

#endregion

public class SetTaskStatusCommand : CommandBase, ISetTaskStatusRequest
{
    #region Properties

    public int Id { get; }

    public int UserId { get; }

    public TaskStatus Status { get; }

    public new bool Result { get; set; }

    #endregion

    #region Constructors

    public SetTaskStatusCommand(int id,
                                TaskStatus status,
                                int userId)
    {
        Id = id;
        Status = status;
        UserId = userId;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    public class Validator : AbstractValidator<SetTaskStatusCommand>
    {
        #region Constructors

        public Validator(IDispatcher dispatcher)
        {
            RuleFor(r => r.Id).NotEmpty().WithMessage(Localization.Task_id_cant_be_empty)
                              .MustAsync((command, _, _) => dispatcher.QueryAsync(new DoesEntityBelongToUserQuery<TaskEntity>(command.Id, command.UserId)))
                              .WithMessage(Localization.Task_id_is_invalid);
        }

        #endregion
    }

    [UsedImplicitly]
    class Handler : CommandHandlerBase<SetTaskStatusCommand>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(SetTaskStatusCommand command, CancellationToken cancellationToken)
        {
            var task = await Repository.Read(new UserIdProp.FindBy.EqualTo<TaskEntity>(command.UserId) &&
                                             new FindEntityByIntId<TaskEntity>(command.Id))
                                       .SingleAsync(cancellationToken);

            task.Status = command.Status;

            await Repository.UpdateAsync(task, cancellationToken);

            command.Result = true;
        }
    }

    #endregion
}