﻿namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using Extensions;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Samples.ToDo.Shared;

#endregion

public class CreateOrUpdateTaskCommand : CommandBase
{
    #region Properties

    public int? Id { get; }

    public int UserId { get; }

    public int ProjectId { get; }

    public string Name { get; }

    public string Description { get; }

    public DateTime? DueDate { get; }

    public int[] TagsIds { get; }

    #endregion

    #region Constructors

    public CreateOrUpdateTaskCommand(int? id,
                                     int userId,
                                     int projectId,
                                     string name,
                                     string description,
                                     DateTime? dueDate,
                                     IEnumerable<int> tagsIds)
    {
        Id = id;
        UserId = userId;
        ProjectId = projectId;
        Name = name.Trim();
        Description = description.Trim();
        DueDate = dueDate;
        TagsIds = tagsIds.ToDistinctArrayOrEmpty();
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    public class Validator : AbstractValidator<CreateOrUpdateTaskCommand>
    {
        #region Constructors

        public Validator(IDispatcher dispatcher)
        {
            When(r => r.Id != null,
                 () =>
                 {
                     RuleFor(r => r.Id).MustAsync((id, _) => dispatcher.QueryAsync(new DoesEntityExistQuery<TaskEntity>(id.Value)))
                                       .WithMessage(ValidationMessagesConst.Invalid_task_id);
                 });

            RuleFor(r => r.UserId).NotEmpty()
                                  .MustAsync((userId, _) => dispatcher.QueryAsync(new DoesEntityExistQuery<UserEntity>(userId)))
                                  .WithMessage(ValidationMessagesConst.Invalid_user_id);

            RuleFor(r => r.ProjectId).NotEmpty()
                                     .MustAsync((projectId, _) => dispatcher.QueryAsync(new DoesEntityExistQuery<ProjectEntity>(projectId)))
                                     .WithMessage(ValidationMessagesConst.Invalid_project_id);

            RuleFor(r => r.Name).NotEmpty()
                                .MustAsync((command, _, _) => dispatcher.QueryAsync(new IsNameUniqueQuery<ProjectEntity>(command.Id, command.UserId, command.Name)))
                                .WithMessage(ValidationMessagesConst.Name_is_not_unique);
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
            var task = command.Id == null ?
                               null :
                               await Repository.Read(new FindEntityByIntId<TaskEntity>(command.Id.Value)).SingleOrDefaultAsync(cancellationToken);

            var isNew = task == null;
            if (isNew)
                task = new TaskEntity { UserId = command.UserId };

            task.Name = command.Name;
            task.Description = command.Description;
            task.UpDt = DateTime.UtcNow;
            task.DueDate = command.DueDate;
            task.ProjectId = command.ProjectId;

            if (isNew)
            {
                await Repository.CreateAsync(task, cancellationToken);

                await Repository.CreateAsync(command.TagsIds.Select(tagId => new ProjectToTagEntity
                                                                             {
                                                                                     TagId = tagId,
                                                                                     ProjectId = task.Id
                                                                             }), cancellationToken);
            }
            else
            {
                await Repository.UpdateAsync(task, cancellationToken);

                var existingTagsMaps = await Repository.Read(new UserIdProp.FindBy.EqualTo<TaskToTagEntity>(command.UserId) &&
                                                             new TaskIdProp.FindBy.EqualTo<TaskToTagEntity>(task.Id))
                                                       .Select(r => new
                                                                    {
                                                                            r.Id,
                                                                            r.TagId
                                                                    }).ToArrayAsync(cancellationToken);

                var existingTagsIds = existingTagsMaps.Select(r => r.TagId).ToArray();
                var tagsIdsToCreate = command.TagsIds.Where(tagId => !existingTagsIds.Contains(tagId)).ToArray();

                await Repository.CreateAsync(tagsIdsToCreate.Select(tagId => new TaskToTagEntity
                                                                             {
                                                                                     TaskId = task.Id,
                                                                                     TagId = tagId
                                                                             }), cancellationToken);

                var tagsMapsToDelete = existingTagsMaps.Where(r => !command.TagsIds.Contains(r.TagId)).ToArray();
                var tagsToDelete = await Repository.Read(new FindEntitiesByIds<TaskToTagEntity, int>(tagsMapsToDelete.Select(r => r.Id))).ToArrayAsync(cancellationToken);

                await Repository.DeleteAsync(tagsToDelete, cancellationToken);
            }
        }
    }

    #endregion
}