namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Samples.ToDo.Shared;

#endregion

public class SetTaskStatusCommand : CommandBase
{
    #region Properties

    public int Id { get; }

    public int StatusId { get; }

    #endregion

    #region Constructors

    public SetTaskStatusCommand(int id, int statusId)
    {
        Id = id;
        StatusId = statusId;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    public class Validator : AbstractValidator<SetTaskStatusCommand>
    {
        #region Constructors

        public Validator(IDispatcher dispatcher)
        {
            RuleFor(r => r.Id).NotEmpty()
                              .MustAsync((id, _) => dispatcher.QueryAsync(new DoesEntityExistQuery<TaskEntity>(id)))
                              .WithMessage(ValidationMessagesConst.Invalid_task_id);

            RuleFor(r => r.StatusId).NotEmpty()
                                    .MustAsync((statusId, _) => dispatcher.QueryAsync(new DoesEntityExistQuery<StatusEntity>(statusId)))
                                    .WithMessage(ValidationMessagesConst.Invalid_status_id);
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
            var task = await Repository.Read(new FindEntityByIntId<TaskEntity>(command.Id)).SingleAsync(cancellationToken);

            task.StatusId = command.StatusId;

            await Repository.UpdateAsync(task, cancellationToken);
        }
    }

    #endregion
}