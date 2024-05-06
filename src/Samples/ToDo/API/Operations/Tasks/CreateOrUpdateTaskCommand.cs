namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Samples.ToDo.Shared;

#endregion

public class CreateOrUpdateTaskCommand : CommandBase, ICreateOrUpdateTaskRequest
{
    #region Properties

    public int? Id { get; }

    public int UserId { get; }

    public int ProjectId { get; }

    public TaskStatus Status { get; }

    public string Name { get; }

    public string Description { get; }

    public new bool Result { get; set; }

    #endregion

    #region Constructors

    public CreateOrUpdateTaskCommand(int? id,
                                     int userId,
                                     int projectId,
                                     TaskStatus status,
                                     string name,
                                     string description)
    {
        Id = id;
        UserId = userId;
        ProjectId = projectId;
        Name = name?.Trim() ?? string.Empty;
        Description = description?.Trim() ?? string.Empty;
        Status = status;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    public class Validator : AbstractValidator<CreateOrUpdateTaskCommand>
    {
        #region Constructors

        public Validator(IDispatcher dispatcher)
        {
            RuleFor(r => r.Id).MustAsync((command, _, _) => dispatcher.QueryAsync(new DoesEntityBelongToUserQuery<TaskEntity>(command.Id, command.UserId)))
                              .WithMessage(Localization.Task_id_is_invalid);

            RuleFor(r => r.UserId).NotEmpty().WithMessage(Localization.User_id_cant_be_empty)
                                  .MustAsync((userId, _) => dispatcher.QueryAsync(new DoesEntityExistQuery<UserEntity>(userId)))
                                  .WithMessage(Localization.User_id_is_invalid);

            RuleFor(r => r.ProjectId).NotEmpty().WithMessage(Localization.Project_id_cant_be_empty)
                                     .MustAsync((projectId, _) => dispatcher.QueryAsync(new DoesEntityExistQuery<ProjectEntity>(projectId)))
                                     .WithMessage(Localization.Project_id_is_invalid);

            RuleFor(r => r.Name).NotEmpty().WithMessage(Localization.Name_cant_be_empty)
                                .MustAsync((command, _, _) => dispatcher.QueryAsync(new IsNameUniqueQuery<TaskEntity>(command.Id, command.UserId, command.Name)))
                                .WithMessage(Localization.Name_is_not_unique);
        }

        #endregion
    }

    [UsedImplicitly]
    class Handler : CommandHandlerBase<CreateOrUpdateTaskCommand>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(CreateOrUpdateTaskCommand command, CancellationToken cancellationToken)
        {
            if (string.Equals(command.Name, TestLiterals.Test_failure, StringComparison.InvariantCultureIgnoreCase))
            {
                command.Result = false;
                return;
            }

            var task = command.Id == null ?
                               null :
                               await Repository.Read(new FindEntityByIntId<TaskEntity>(command.Id.Value)).SingleOrDefaultAsync(cancellationToken);

            var isNew = task == null;
            if (isNew)
                task = new TaskEntity
                       {
                               UserId = command.UserId,
                               ProjectId = command.ProjectId
                       };

            task.Name = command.Name;
            task.Description = command.Description;
            task.UpDt = DateTime.UtcNow;
            task.Status = command.Status;

            if (isNew)
                await Repository.CreateAsync(task, cancellationToken);
            else
                await Repository.UpdateAsync(task, cancellationToken);

            command.Result = true;
        }
    }

    #endregion
}